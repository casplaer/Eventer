using Eventer.Infrastructure.Validators;
using FluentValidation;

namespace Eventer.API.Extensions
{
    public static class ValidatorsExtensions
    {
        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<EventCategoryValidator>();
            services.AddValidatorsFromAssemblyContaining<UserValidator>();
            services.AddValidatorsFromAssemblyContaining<EventValidator>();
            services.AddValidatorsFromAssemblyContaining<EventRegistrationValidator>();

            return services;
        }
    }
}
