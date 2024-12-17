using Microsoft.OpenApi.Models;

namespace Eventer.API.Extensions
{
    public static class SwaggerServiceExtensions
    {
        public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Eventer WEB API",
                    Version = "v1",
                    Description = "Данное API является тестовым заданием на позицию .NET Intern.",
                });

                options.MapType<DateOnly>(() => new OpenApiSchema
                {
                    Type = "string",
                    Format = "date",
                });

                options.MapType<TimeOnly>(() => new OpenApiSchema
                {
                    Type = "string",
                    Format = "time",
                });
            });

            return services;
        }
    }
}
