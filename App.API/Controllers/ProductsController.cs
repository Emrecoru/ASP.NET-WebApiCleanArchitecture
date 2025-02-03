using App.API.Filters;
using App.Application.Features.Products;
using App.Application.Features.Products.Create;
using App.Application.Features.Products.Update;
using App.Application.Features.Products.UpdateStock;
using App.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers
{
    public class ProductsController : CustomControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var serviceResult = await _productService.GetAllListAsync();

            return CreateActionResult(serviceResult);
        }

        [HttpGet("{pageNumber:int}/{pageSize:int}")]
        public async Task<IActionResult> GetPagedAll(int pageNumber, int pageSize)
        {
            var serviceResult = await _productService.GetPagedAllListAsync(pageNumber, pageSize);

            return CreateActionResult(serviceResult);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var serviceResult = await _productService.GetByIdAsync(id);

            return CreateActionResult(serviceResult);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductRequest request)
        {
            var serviceResult =  await _productService.CreateAsync(request);

            return CreateActionResult(serviceResult);
        }

        //[ServiceFilter(typeof(NotFoundFilter<Product, int>))]
        [HttpPut]
        public async Task<IActionResult> Update(UpdateProductRequest request)
        {
            var serviceResult = await _productService.UpdateAsync(request);

            return CreateActionResult(serviceResult);
        }

        [HttpPatch("stock")]
        public async Task<IActionResult> UpdateStock(UpdateProductStockRequest request)
        {
            return CreateActionResult(await _productService.UpdateStockAsync(request));
        }

        //[HttpPut("UpdateStock")]
        //public async Task<IActionResult> UpdateStock(UpdateProductStockRequest request)
        //{
        //    return CreateActionResult(await _productService.UpdateStockAsync(request));
        //}

        [ServiceFilter(typeof(NotFoundFilter<Product, int>))]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var serviceResult = await _productService.DeleteAsync(id);

            return CreateActionResult(serviceResult);
        }
    }
}
