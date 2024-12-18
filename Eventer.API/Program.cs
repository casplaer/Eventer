using Eventer.API.Extensions;
using Eventer.Infrastructure.Data;
using Eventer.Infrastructure.Migrations;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCustomSwagger();
builder.Services.AddCustomCors();

builder.Services.AddDbContext<EventerDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddValidators();
builder.Services.AddApplicationRepositories();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddApplicationUseCases(builder.Configuration);
builder.Services.AddGeneralServices();

var app = builder.Build();

DbInitializer.SeedDatabase(app.Services);

app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseCors("AllowReactClient");
app.UseAuthentication();
app.UseAuthorization();

app.UseCustomMiddlewares();

app.MapControllers();
app.Run();
