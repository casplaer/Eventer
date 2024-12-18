using Eventer.Application.Interfaces;
using Eventer.Application.Interfaces.Auth;
using Eventer.Application.Interfaces.Services;
using Eventer.Application.Mappings;
using Eventer.Infrastructure;
using Eventer.Infrastructure.Data;
using Eventer.Infrastructure.Services;
using Eventer.Infrastructure.Validators;

namespace Eventer.API.Extensions
{
    public static class GeneralExtensions
    {
        public static IServiceCollection AddGeneralServices(this IServiceCollection services)
        {
            services.AddScoped<IImageService, ImageService>();

            services.AddAutoMapper(typeof(UpdateEventProfile));
            services.AddAutoMapper(typeof(CreateEventProfile));
            services.AddAutoMapper(typeof(UpdateEnrollmentProfile));
            services.AddAutoMapper(typeof(CategoryProfile));
            services.AddAutoMapper(typeof(UserProfile));
            services.AddAutoMapper(typeof(CreateEnrollmentProfile));
            services.AddAutoMapper(typeof(EventProfile));

            services.AddScoped<IUniqueFieldChecker, UniqueFieldChecker>();

            services.AddScoped<IPasswordHasher, PasswordHasher>();

            services.AddScoped<IJwtProvider, JwtProvider>();

            return services;
        }
    }
}
