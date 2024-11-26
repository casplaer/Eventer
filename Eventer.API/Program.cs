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
        Description = "������ API �������� �������� �������� �� ������� .NET Intern.",
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

//���� ������
builder.Services.AddDbContext<EventerDbContext>(options=>
    options.UseNpgsql(connectionString));

//UnitOfWork � �����������
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));//����� ���������� �����������
builder.Services.AddScoped<IEventRepository, EventRepository>();//����������� �������
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();//����������� ���������
builder.Services.AddScoped<IUserRepository, UserRepository>();//����������� �������������

//�������
builder.Services.AddScoped<IEventService, EventService>();//������ ��� �������
builder.Services.AddScoped<ICategoryService, CategoryService>();//������ ��� ���������
builder.Services.AddScoped<IAuthService,  AuthService>();//������ ��� ��������������
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();//������ ��� ������� �� �������

builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();//�����

builder.Services.AddHttpClient<IEventService, EventService>(opt=>opt.BaseAddress = new Uri(builder.Configuration.GetSection("ApiSettings")["BaseUrl"]));

//CORS ��� ������� ������� � API
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

//������������ JWT
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
