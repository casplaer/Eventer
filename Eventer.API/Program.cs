using Eventer.Application.Interfaces.Repositories;
using Eventer.Application.Interfaces.Services;
using Eventer.Application.Services;
using Eventer.Domain.Models;
using Eventer.Infrastructure.Data;
using Eventer.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

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
        Example = new OpenApiString("2024-11-19")
    });
});

//База данных
builder.Services.AddDbContext<EventsDbContext>(options=>
    options.UseNpgsql(connectionString));

//UnitOfWork и репозитории
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));//Общая реализация репозитория
builder.Services.AddScoped<IEventRepository, EventRepository>();//Репозиторий событий
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();//Репозиторий категорий

//Сервисы
builder.Services.AddScoped<IEventService, EventService>();//Сервис для событий

var app = builder.Build();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

var context = services.GetRequiredService<EventsDbContext>();
await context.Database.EnsureCreatedAsync();
SeedData(context);

void SeedData(EventsDbContext context)
{
    if (!context.Categories.Any())
    {
        context.Categories.Add(new EventCategory
        {
            Id = Guid.NewGuid(),
            Name = "Test Category"
        });

        context.SaveChanges();
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
