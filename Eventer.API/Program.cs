using Eventer.Application.Interfaces.Auth;
using Eventer.Application.Interfaces.Repositories;
using Eventer.Application.Interfaces.Services;
using Eventer.Application.Services;
using Eventer.Domain.Models;
using Eventer.Infrastructure;
using Eventer.Infrastructure.Data;
using Eventer.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using System.Net.NetworkInformation;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
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
        Format= "time",
    });
});

//База данных
builder.Services.AddDbContext<EventerDbContext>(options=>
    options.UseNpgsql(connectionString));

//UnitOfWork и репозитории
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));//Общая реализация репозитория
builder.Services.AddScoped<IEventRepository, EventRepository>();//Репозиторий событий
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();//Репозиторий категорий
builder.Services.AddScoped<IUserRepository, UserRepository>();//Репозиторий пользователей

//Сервисы
builder.Services.AddScoped<IEventService, EventService>();//Сервис для событий
builder.Services.AddScoped<ICategoryService, CategoryService>();//Сервис для категорий
builder.Services.AddScoped<IAuthService,  AuthService>();//Сервис для аутентификации

builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();//Хэшер

builder.Services.AddCors(options=>
{
    options.AddPolicy("AllowSpecificOrigins",policy =>
    {
        policy.WithOrigins("http://localhost:5173") 
                  .AllowAnyHeader()
                  .AllowAnyMethod();
    });
}
);

var app = builder.Build();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
    var context = services.GetRequiredService<EventerDbContext>();
    var hasher = services.GetRequiredService<IPasswordHasher>();
    context.Database.Migrate();
    DbInitializer.Initialize(context, hasher);
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred while migrating the database.");
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors("AllowSpecificOrigins");

app.Run();
