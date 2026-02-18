namespace GrocerySupermarket.WebAPI.Middleware;

public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private const string HeaderName = "X-API-KEY";

    // 🔓 PUBLIC ENDPOINT PREFIXES (NO API KEY)
    private static readonly string[] PublicPrefixes =
    {
        "/api/auth",
        "/api/product",
        "/api/category",
        "/api/category-promos",
        "/api/banners",
        "/api/orders",          // GET only (JWT handles auth)
        "/api/invoices",        // JWT only
        "/swagger",
        "/"
    };

    public ApiKeyMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.Value?.ToLower();

        // ✅ Skip API key for public routes
        if (path != null && PublicPrefixes.Any(p => path.StartsWith(p)))
        {
            await _next(context);
            return;
        }

        // 🔐 Enforce API key for protected routes
        if (!context.Request.Headers.TryGetValue(HeaderName, out var providedKey))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("API Key is missing.");
            return;
        }

        var expectedKey = Environment.GetEnvironmentVariable("API_KEY");

        if (string.IsNullOrWhiteSpace(expectedKey) || providedKey != expectedKey)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Invalid API Key.");
            return;
        }

        await _next(context);
    }
}
