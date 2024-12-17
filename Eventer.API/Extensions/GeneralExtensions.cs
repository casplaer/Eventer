using Eventer.Application.Interfaces;
using Eventer.Application.Interfaces.Auth;
using Eventer.Application.Interfaces.Services;
using Eventer.Application.Mappings;
using Eventer.Application.Services;
using Eventer.Infrastructure;
using Eventer.Infrastructure.Data;
using Eventer.Infrastructure.Validators;

namespace Eventer.API.Extensions
{
    public static class GeneralExtensions
    {
        public static IServiceCollection AddGeneralServices(this IServiceCollection services)
        {
            services.AddScoped<IImageService, ImageService>();

            services.AddAutoMapper(typeof(EventProfile).Assembly);
            services.AddAutoMapper(typeof(EnrollmentProfile).Assembly);
            services.AddAutoMapper(typeof(CategoryProfile).Assembly);

            services.AddScoped<IUniqueFieldChecker, UniqueFieldChecker>();

            services.AddScoped<IPasswordHasher, PasswordHasher>();

            services.AddScoped<IJwtProvider, JwtProvider>();

            return services;
        }
    }
}
