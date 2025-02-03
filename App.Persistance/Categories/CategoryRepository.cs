using App.Application.Contracts.Persistance;
using App.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace App.Persistance
{
    public class CategoryRepository : GenericRepository<Category, int>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context)
        {
        }
        public async  Task<Category?> GetCategoryWithProductsAsync(int id)
        {
            var category = await Context.Categories.Include(x => x.Products).SingleOrDefaultAsync(x => x.Id == id);

            return category;
        }

        public IQueryable<Category> GetCategoryWithProducts()
        {
            var categories = Context.Categories.Include(x => x.Products).AsQueryable();

            return categories;
        }

        public async Task<List<Category>> GetCategoryWithProductsAsync()
        {
            var categories = await Context.Categories.Include(x => x.Products).ToListAsync();

            return categories;
        }
    }
}
