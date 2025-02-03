using App.Application.Features.Categories;
using App.Application.Features.Products;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace App.Application.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();

            services.AddScoped<ICategoryService, CategoryService>();
            
            //services.AddFluentValidationAutoValidation();

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            //services.AddExceptionHandler<CriticalExceptionHandler>();

            //services.AddExceptionHandler<GlobalExceptionHandler>();

            //services.AddScoped(typeof(NotFoundFilter<,>));

            return services;
        }
    }
}
