using App.API.Extensions;
using App.Application.Extensions;
using App.Persistance.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViewsExt().AddSwaggerGenExt().AddExceptionHandlerExt().AddCachingExt();

builder.Services.AddRepository(builder.Configuration).AddServices();

//builder.Services.AddScoped(typeof(NotFoundFilter<,>));

//builder.Services.AddExceptionHandler<CriticalExceptionHandler>();
//builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

//builder.Services.AddMemoryCache();
//builder.Services.AddSingleton<ICacheService, CacheService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddSwaggerGenExt();

var app = builder.Build();

app.UseConfigurePipelineExt();

app.MapControllers();

app.Run();
