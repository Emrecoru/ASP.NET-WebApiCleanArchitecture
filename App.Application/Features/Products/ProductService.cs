using App.Application.Contracts.Caching;
using App.Application.Contracts.Persistance;
using App.Application.Features.Products.Create;
using App.Application.Features.Products.Dto;
using App.Application.Features.Products.Update;
using App.Application.Features.Products.UpdateStock;
using App.Domain.Entities;
using AutoMapper;
using FluentValidation;
using System.Net;

namespace App.Application.Features.Products
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateProductRequest> _createProductRequestValidator;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateProductRequest> _updateProductRequestValidator;
        private readonly ICacheService _cacheService;

        private const string ProductListCacheKey = "ProductListCacheKey";

        public ProductService(IProductRepository productRepository, IUnitOfWork unitOfWork, IValidator<CreateProductRequest> createProductRequestValidator, IMapper mapper, IValidator<UpdateProductRequest> updateProductRequestValidator, ICacheService cacheService)
        {
            this.productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _createProductRequestValidator = createProductRequestValidator;
            _mapper = mapper;
            _updateProductRequestValidator = updateProductRequestValidator;
            _cacheService = cacheService;
        }

        public async Task<ServiceResult<List<ProductDto>>> GetTopPriceProductsAsync(int count)
        {
            var products = await productRepository.GetTopPriceProductsAsync(count);

            //var productsAsDto = products.Select(x => new ProductDto(x.Id, x.Name, x.Price * 1.2m, x.Stock)).ToList();

            var productsAsDto = _mapper.Map<List<ProductDto>>(products);
           
            return ServiceResult<List<ProductDto>>.Success(productsAsDto);
        }

        public async Task<ServiceResult<List<ProductDto>>> GetAllListAsync()
        {
            //cache aside design pattern

            // 1. any cache, 2. from db, 3. caching data

            var productListAsCached = await _cacheService.GetAsync<List<ProductDto>>(ProductListCacheKey);
            
            if(productListAsCached is not null)
            {
                return ServiceResult<List<ProductDto>>.Success(productListAsCached);
            }

            var products = await productRepository.GetAllAsync();

            //var productsAsDto = products.Select(x => new ProductDto(x.Id, x.Name, x.Price, x.Stock)).ToList();

            var productsAsDto = _mapper.Map<List<ProductDto>>(products);

            await _cacheService.AddAsync(ProductListCacheKey, productsAsDto, TimeSpan.FromMinutes(5));

            return ServiceResult<List<ProductDto>>.Success(productsAsDto);
        }

        public async Task<ServiceResult<List<ProductDto>>> GetPagedAllListAsync(int pageNumber, int pageSize)
        {
            int skip = (pageNumber - 1) * pageSize;

            var products = await productRepository.GetAllPagedAsync(pageNumber, pageSize);

            //var productsAsDto = products.Select(x => new ProductDto(x.Id, x.Name, x.Price, x.Stock)).ToList();
            
            var productsAsDto = _mapper.Map<List<ProductDto>>(products);

            return ServiceResult<List<ProductDto>>.Success(productsAsDto);
        }

        public async Task<ServiceResult<ProductDto?>> GetByIdAsync(int id)
        {
            var product = await productRepository.GetByIdAsync(id);

            if(product is null)
            {
               return ServiceResult<ProductDto?>.Fail("Product not found", HttpStatusCode.NotFound);
            }

            //var productAsDto = new ProductDto(product.Id, product.Name, product.Price, product.Stock);

            var productAsDto = _mapper.Map<ProductDto>(product);

            return ServiceResult<ProductDto>.Success(productAsDto)!;
        }

        public async Task<ServiceResult<CreateProductResponse>> CreateAsync(CreateProductRequest request)
        {
            //Async manuel service business check
            //var anyProduct = await productRepository.Where(x => x.Name == request.Name).AnyAsync();

            //if (anyProduct)
            //{
            //    return ServiceResult<CreateProductResponse>.Fail("Ürün ismi veritabanında bulunmaktadır");
            //}

            //Async manuel fluent validation business check
            var validationResult = await _createProductRequestValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return ServiceResult<CreateProductResponse>.Fail(validationResult.Errors.Select(x => x.ErrorMessage).ToList());
            }

            //var product = new Product 
            //{
            //    Name = request.Name,
            //    Price = request.Price,
            //    Stock = request.Stock,
            //};

            var product = _mapper.Map<Product>(request);

            await productRepository.AddAsync(product);

            await _unitOfWork.SaveChangesAsync();

            return ServiceResult<CreateProductResponse>.SuccessAsCreated(new CreateProductResponse(product.Id),
                $"api/products/{product.Id}" );
        }

        public async Task<ServiceResult> UpdateAsync(UpdateProductRequest request)
        {
            var product = await productRepository.GetByIdAsync(request.Id);

            if (product is null)
            {
                return ServiceResult.Fail("Product not found.", HttpStatusCode.NotFound);
            }

            var validationResult = await _updateProductRequestValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return ServiceResult.Fail(validationResult.Errors.Select(x => x.ErrorMessage).ToList());
            }

            //product.Name = request.Name;
            //product.Price = request.Price;
            //product.Stock = request.Stock;

            product = _mapper.Map(request, product);

            productRepository.Update(product!);

            await _unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult> UpdateStockAsync(UpdateProductStockRequest updateProductStockRequest)
        {
            var product = await productRepository.GetByIdAsync(updateProductStockRequest.productId);

            if(product is null)
            {
                return ServiceResult.Fail("Product not found", HttpStatusCode.NotFound);
            }

            product.Stock = updateProductStockRequest.quantity;

            productRepository.Update(product);
            await _unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            var product = await productRepository.GetByIdAsync(id);

            //if(product is null)
            //{
            //    return ServiceResult.Fail("Product not found", HttpStatusCode.NotFound);
            //}

            productRepository.Delete(product!);

            await _unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }
    }
}
