﻿using Eventer.Contracts.Requests.Auth;
using Eventer.Contracts.Requests.Enrollments;
using Eventer.Domain.Models;
using Eventer.Infrastructure.Validators.Auth;
using Eventer.Infrastructure.Validators.Categories;
using Eventer.Infrastructure.Validators.Enrollments;
using Eventer.Infrastructure.Validators.Events;
using FluentValidation;
using FluentValidation.AspNetCore;

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
            services.AddScoped<IValidator<EventCategory>, EventCategoryValidator>();
            services.AddControllers()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<UpdateEnrollRequestValidator>())
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<EnrollRequestValidator>())
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<LoginUserRequestValidator>())
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<RegisterUserRequestValidator>());

            return services;
        }
    }
}
