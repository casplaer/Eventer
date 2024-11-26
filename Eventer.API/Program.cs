using Eventer.Application.Interfaces.Auth;
using Eventer.Application.Interfaces.Repositories;
using Eventer.Application.Interfaces.Services;
using Eventer.Application.Services;
using Eventer.Infrastructure;
using Eventer.Infrastructure.Data;
using Eventer.Infrastructure.Middleware;
using Eventer.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

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
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();//Сервис для записей на события

builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();//Хэшер

builder.Services.AddHttpClient<IEventService, EventService>(opt=>opt.BaseAddress = new Uri(builder.Configuration.GetSection("ApiSettings")["BaseUrl"]));

//CORS для доступа клиента к API
builder.Services.AddCors(options=>
{
    options.AddPolicy("AllowReactClient",policy =>
    {
        policy.WithOrigins("http://localhost:5173") 
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
    });
}
);

builder.Services.AddScoped<IJwtProvider, JwtProvider>();

//Конфигурация JWT
builder.Services.AddAuthentication(options=>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options=>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ClockSkew = TimeSpan.Zero,
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy =>
        policy.RequireRole("Admin"));
});

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
    //app.UseSwagger();
    //app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowReactClient");

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();

app.MapControllers();

app.Run();
