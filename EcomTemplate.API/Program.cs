using AspNetCoreRateLimit;
using DotNetEnv;
using EcomTemplate.Application.Interfaces;
using EcomTemplate.Application.Interfaces.Admin;
using EcomTemplate.Application.Interfaces.Service.Admin;
using EcomTemplate.Application.Mapping;
using EcomTemplate.Domain.Entities;
using EcomTemplate.Infrastructure.Data;
using EcomTemplate.Infrastructure.Options;
using EcomTemplate.Infrastructure.Repositories;
using EcomTemplate.Infrastructure.Repositories.Admin;
using EcomTemplate.Infrastructure.Security;
using EcomTemplate.Infrastructure.Services;
using EcomTemplate.Infrastructure.Services.Admin;
using EcomTemplate.WebAPI.Middleware;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using Serilog;
using StackExchange.Redis;
using System.Security.Claims;
using System.Text;
using CloudinaryDotNet;

// =============================================================
// LOAD ENV
// =============================================================
var envPath = Path.Combine(Directory.GetCurrentDirectory(), ".env");
if (File.Exists(envPath))
{
    Env.Load(envPath);
}

static string RequireEnv(string key) =>
    Environment.GetEnvironmentVariable(key)
    ?? throw new InvalidOperationException($"{key} is missing");

// =============================================================
// LOGGING
// =============================================================
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/api-log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

// =============================================================
// DATABASE
// =============================================================
var connectionString =
    $"Host={RequireEnv("DB_HOST")};" +
    $"Port={RequireEnv("DB_PORT")};" +
    $"Database={RequireEnv("DB_NAME")};" +
    $"Username={RequireEnv("DB_USER")};" +
    $"Password={RequireEnv("DB_PASSWORD")}";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString)
);

// =============================================================
// CLOUDINARY
// =============================================================
builder.Services.AddSingleton(_ =>
{
    return new Cloudinary(new Account(
        RequireEnv("CLOUDINARY_CLOUD_NAME"),
        RequireEnv("CLOUDINARY_API_KEY"),
        RequireEnv("CLOUDINARY_API_SECRET")
    ));
});

// =============================================================
// REDIS
// =============================================================
var redisHost = Environment.GetEnvironmentVariable("REDIS_HOST") ?? "localhost";

builder.Services.AddSingleton<IConnectionMultiplexer>(_ =>
{
    var config = ConfigurationOptions.Parse($"{redisHost}:6379", true);
    config.AbortOnConnectFail = false;
    return ConnectionMultiplexer.Connect(config);
});

builder.Services.AddSingleton<CacheService>();

// =============================================================
// DEPENDENCY INJECTION
// =============================================================

// ===== PRODUCT & CATEGORY =====
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IBannerRepository, BannerRepository>();

// ===== AUTH (CUSTOMER) =====
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<CustomerProfileService>();
builder.Services.AddScoped<ICustomerProfileRepository, CustomerProfileRepository>();

// ===== CUSTOMER CART & ORDER =====
builder.Services.AddScoped<ICustomerCartRepository, CustomerCartRepository>();
builder.Services.AddScoped<ICustomerCheckoutService, CustomerCheckoutService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, CustomerOrderService>();

// ===== PAYMENT =====
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();

// ===== ADMIN =====
builder.Services.AddScoped<IAdminAuthService, AdminAuthService>();
builder.Services.AddScoped<AdminTokenService>();

builder.Services.AddScoped<IDashboardRespository, DashboardRespository>();
builder.Services.AddScoped<IDashboardService, DashboardService>();


builder.Services.AddScoped<IAddProducts, AddProducts>();

// ===== AUTOMAPPER =====
builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

// ===== OPTIONS =====
builder.Services.Configure<CheckoutDefaultsOptions>(
    builder.Configuration.GetSection("CheckoutSettings")
);

// =============================================================
// JWT AUTHENTICATION
// =============================================================
var jwtKey = RequireEnv("JWT_KEY");
var jwtIssuer = RequireEnv("JWT_ISSUER");
var jwtAudience = RequireEnv("JWT_AUDIENCE");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey)
            ),

            RoleClaimType = ClaimTypes.Role
        };
    });

// =============================================================
// RATE LIMITING
// =============================================================
builder.Services.AddMemoryCache();

builder.Services.Configure<IpRateLimitOptions>(options =>
{
    options.GeneralRules = new List<RateLimitRule>
    {
        new RateLimitRule
        {
            Endpoint = "*",
            Period = "1m",
            Limit = 100
        }
    };
});

builder.Services.AddInMemoryRateLimiting();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

// =============================================================
// CONTROLLERS
// =============================================================
builder.Services.AddControllers();

// =============================================================
// SWAGGER
// =============================================================
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "EcomTemplate API",
        Version = "v1"
    });

    // JWT
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header
    });

    // API KEY
    options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Name = "X-API-KEY",
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        },
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                }
            },
            Array.Empty<string>()
        }
    });
});

// =============================================================
// CORS
// =============================================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3001")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// =============================================================
// BUILD APP
// =============================================================
var app = builder.Build();

// =============================================================
// MIDDLEWARE
// =============================================================
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "EcomTemplate API v1");
        c.RoutePrefix = string.Empty;
    });
}
else
{
    app.UseExceptionHandler("/error");
    app.UseHttpsRedirection();
}

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.UseIpRateLimiting();

app.UseMiddleware<ApiKeyMiddleware>();

app.MapControllers();

app.Run();