using Eventer.Application.Interfaces.Auth;
using Eventer.Application.Interfaces.Services;
using Eventer.Application.Interfaces.UseCases.Auth;
using Eventer.Application.Interfaces.UseCases.Category;
using Eventer.Application.Interfaces.UseCases.Enrollment;
using Eventer.Application.Interfaces.UseCases.Events;
using Eventer.Application.Mappings;
using Eventer.Application.Services;
using Eventer.Application.UseCases.Auth;
using Eventer.Application.UseCases.Category;
using Eventer.Application.UseCases.Enrollment;
using Eventer.Application.UseCases.Events;
using Eventer.Infrastructure.Data;
using Eventer.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Eventer.API.Extensions
{
    public static class UseCasesCollectionExtensions
    {
        public static IServiceCollection AddApplicationUseCases(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddScoped<IImageService, ImageService>();

            services.AddAutoMapper(typeof(EventProfile).Assembly);
            services.AddAutoMapper(typeof(EnrollmentProfile).Assembly);
            services.AddAutoMapper(typeof(CategoryProfile).Assembly);

            services.AddScoped<IPasswordHasher, PasswordHasher>();

            services.AddScoped<IJwtProvider, JwtProvider>();

            services.AddEventUseCases(configuration);
            services.AddCategoryUseCases();
            services.AddAuthUseCases();
            services.AddEnrollmentUseCases();

            return services;
        }

        private static IServiceCollection AddEventUseCases(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddHttpClient<IAddEventUseCase, AddEventUseCase>(opt =>
                opt.BaseAddress = new Uri(configuration.GetSection("ApiSettings")["BaseUrl"]));

            services.AddHttpClient<IUpdateEventUseCase, UpdateEventUseCase>(opt =>
                opt.BaseAddress = new Uri(configuration.GetSection("ApiSettings")["BaseUrl"]));

            services.AddScoped<IDeleteEventUseCase, DeleteEventUseCase>();
            services.AddScoped<IGetAllEventsUseCase, GetAllEventsUseCase>();
            services.AddScoped<IGetEventByIdUseCase, GetEventByIdUseCase>();
            services.AddScoped<IGetFilteredEventsUseCase, GetFilteredEventsUseCase>();
            services.AddScoped<IGetUsersEventsUseCase, GetUsersEventsUseCase>();

            return services;
        }

        private static IServiceCollection AddCategoryUseCases(this IServiceCollection services)
        {
            services.AddScoped<IGetAllCategoriesUseCase, GetAllCategoriesUseCase>();
            services.AddScoped<IGetCategoryByIdUseCase, GetCategoryByIdUseCase>();
            services.AddScoped<IAddCategoryUseCase, AddCategoryUseCase>();
            services.AddScoped<IUpdateCategoryUseCase, UpdateCategoryUseCase>();
            services.AddScoped<IDeleteCategoryUseCase, DeleteCategoryUseCase>();
            services.AddScoped<IGetCategoriesByNameUseCase, GetCategoriesByNameUseCase>();
            return services;
        }

        private static IServiceCollection AddAuthUseCases(this IServiceCollection services)
        {
            services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
            services.AddScoped<ILoginUserUseCase, LoginUserUseCase>();
            services.AddScoped<IRefreshTokenUseCase, RefreshTokenUseCase>();
            services.AddScoped<IGetUserByTokenUseCase, GetUserByTokenUseCase>();
            services.AddScoped<IGetUserByUsernameUseCase, GetUserByUsernameUseCase>();
            return services;
        }

        private static IServiceCollection AddEnrollmentUseCases(this IServiceCollection services)
        {
            services.AddScoped<IEnrollOnEventUseCase, EnrollOnEventUseCase>();
            services.AddScoped<ICheckUserEnrollmentUseCase, CheckUserEnrollmentUseCase>();
            services.AddScoped<IUpdateEnrollmentUseCase, UpdateEnrollmentUseCase>();
            services.AddScoped<IDeleteEnrollmentUseCase, DeleteEnrollmentUseCase>();
            services.AddScoped<IGetAllEnrollmentsUseCase, GetAllEnrollmentsUseCase>();
            services.AddScoped<IGetSingleEnrollmentByIdUseCase, GetSingleEnrollmentByIdUseCase>();
            return services;
        }
    }   
}
