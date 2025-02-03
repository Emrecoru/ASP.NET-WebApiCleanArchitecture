using App.API.Filters;

namespace App.API.Extensions
{
    public static class ControllerExtensions
    {
        public static IServiceCollection AddControllersWithViewsExt(this IServiceCollection services)
        {
            services.AddScoped(typeof(NotFoundFilter<,>));

            services.AddControllers(opt =>
            {
                opt.Filters.Add<FluentValidationFilter>();
                opt.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
            });

            return services;
        }
    }
}
