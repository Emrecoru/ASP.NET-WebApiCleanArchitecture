using App.API.ExceptionHandlers;

namespace App.API.Extensions
{
    public static class ExceptionHandlerExtensions
    {
        public static IServiceCollection AddExceptionHandlerExt(this IServiceCollection services)
        {
            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddExceptionHandler<CriticalExceptionHandler>();

            return services;
        }
    }
}
