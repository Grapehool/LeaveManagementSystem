using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Services;
using Persistence.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Persistence.Data;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var key = builder.Configuration["authKey"];

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            ValidIssuer = "http://localhost:5017/api/auth",
            ValidAudience = "http://localhost:5017/api"
        };
    });
builder.Services.AddControllers();

// Swagger for API documentation
builder.Services.AddSwaggerGen();

// Repositories
builder.Services.AddScoped<ILeaveRequestRepository, LeaveRequestRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<ILeaveBalanceRepository, LeaveBalanceRepository>();
builder.Services.AddScoped<IHolidayRepository, HolidayRepository>();

// Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ILeaveService, LeaveService>();
builder.Services.AddScoped<IManagerService, ManagerService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Serves the JSON endpoint
    app.UseSwaggerUI(); // Serves the interactive HTML UI
}

app.UseAuthentication();    
app.UseAuthorization();

app.MapControllers();

app.Run();