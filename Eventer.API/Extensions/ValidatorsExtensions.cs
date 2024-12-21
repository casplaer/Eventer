using Eventer.Domain.Models;
using Eventer.Infrastructure.Validators.Auth;
using Eventer.Infrastructure.Validators.Categories;
using Eventer.Infrastructure.Validators.Enrollments;
using Eventer.Infrastructure.Validators.Events;
using FluentValidation;
using Eventer.Domain.Contracts.Auth;
using Eventer.Domain.Contracts.Enrollments;

namespace Eventer.API.Extensions
{
    public static class ValidatorsExtensions
    {
        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<RegisterUserRequest>, RegisterUserRequestValidator>();
            services.AddScoped<IValidator<EnrollRequest>, EnrollRequestValidator>();
            services.AddScoped<IValidator<UpdateEnrollRequest>, UpdateEnrollRequestValidator>();
            services.AddScoped<IValidator<LoginUserRequest>, LoginUserRequestValidator>();
            services.AddScoped<IValidator<Event>, EventValidator>();
            services.AddScoped<IValidator<User>, UserValidator>();
            services.AddScoped<IValidator<EventCategory>, EventCategoryValidator>();
            services.AddScoped<IValidator<string>, RefreshTokenValidator>();

            return services;
        }
    }
}
