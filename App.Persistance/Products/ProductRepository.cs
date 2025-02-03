using App.Application.Contracts.Persistance;
using App.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace App.Persistance
{
    public class ProductRepository : GenericRepository<Product, int>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Product>> GetTopPriceProductsAsync(int count)
        {
            return await Context.Products.OrderByDescending(x => x.Price).Take(5).ToListAsync();
        }
    }
}
