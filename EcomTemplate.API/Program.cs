using AspNetCoreRateLimit;
using DotNetEnv;
using GrocerySupermarket.Application.Interfaces;
using GrocerySupermarket.Application.Mapping;
using GrocerySupermarket.Domain.Entities;
using GrocerySupermarket.Infrastructure.Data;
using GrocerySupermarket.Infrastructure.Options;
using GrocerySupermarket.Infrastructure.Repositories;
using GrocerySupermarket.Infrastructure.Security;
using GrocerySupermarket.Infrastructure.Services;
using GrocerySupermarket.WebAPI.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using StackExchange.Redis;
using System.Text;
using CloudinaryDotNet;

// =============================================================
// LOAD .env (EXPLICIT PATH – SAFE)
// =============================================================
var envPath = Path.Combine(Directory.GetCurrentDirectory(), ".env");
if (File.Exists(envPath))
{
    Env.Load(envPath);
}





// =============================================================
// HELPERS
// =============================================================
static string RequireEnv(string key) =>
    Environment.GetEnvironmentVariable(key)
    ?? throw new InvalidOperationException($"{key} is missing");

// =============================================================
// SERILOG
// =============================================================
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/api-log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

// =============================================================
// DATABASE (POSTGRESQL)
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

builder.Services.AddSingleton(_ =>
{
    return new Cloudinary(new Account(
        Environment.GetEnvironmentVariable("CLOUDINARY_CLOUD_NAME"),
        Environment.GetEnvironmentVariable("CLOUDINARY_API_KEY"),
        Environment.GetEnvironmentVariable("CLOUDINARY_API_SECRET")
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

// ===== AUTH & PROFILE =====
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<CustomerProfileService>();
builder.Services.AddScoped<ICustomerProfileRepository, CustomerProfileRepository>();


// ===== CUSTOMER CART & ORDER =====
builder.Services.AddScoped<ICustomerCartRepository, CustomerCartRepository>();
builder.Services.AddScoped<ICustomerCheckoutService, CustomerCheckoutService>();
builder.Services.AddScoped<IOrderService, CustomerOrderService>(); // customer orders
builder.Services.AddScoped<IOrderRepository, OrderRepository>();   // shared orders

// ===== GUEST CART & ORDER =====





// Customer

builder.Services.AddScoped<IOrderService, CustomerOrderService>();
builder.Services.AddScoped<ICustomerCartRepository, CustomerCartRepository>();

// Guest







builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

builder.Services.Configure<CheckoutDefaultsOptions>(
    builder.Configuration.GetSection("CheckoutSettings"));


// =============================================================
// JWT AUTHENTICATION (USER AUTH)
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
            )
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
// SWAGGER (JWT + API KEY SUPPORT)
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
        In = ParameterLocation.Header,
        Description = "API Key required for all protected endpoints"
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
// BUILD APP
// =============================================================
var app = builder.Build();

// =============================================================
// MIDDLEWARE PIPELINE (PRODUCTION ORDER)
// =============================================================
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Sanvarich API v1");
        options.RoutePrefix = string.Empty;
    });
}
else
{
    app.UseExceptionHandler("/error");
    app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseIpRateLimiting();

// 🔐 API KEY PROTECTION (AUTH ROUTES WHITELISTED INSIDE MIDDLEWARE)
app.UseMiddleware<ApiKeyMiddleware>();

app.MapControllers();

app.Run();
