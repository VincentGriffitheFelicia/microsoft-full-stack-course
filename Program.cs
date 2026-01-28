var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(5294); // http only
});

var app = builder.Build();

// app.Use(async (context, next) =>
// {
//     if (context.Request.Query["secure"] != true)
//     {
//         context.Response.StatusCode = StatusCodes.Status400BadRequest;
//         await context.Response.WriteAsync("Simulated HTTPS required.");
//         return;
//     }
//     await next();
// });

app.Use(async (context, next) =>
{
    var input = context.Request.Query["input"];
    if (!IsValidInput(input))
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        await context.Response.WriteAsync("Invalid input.");
        return;
    }
    await next();
});

app.Use(async (context, next) =>
{
    if (context.Request.Path == "/unauthorized")
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        await context.Response.WriteAsync("Unauthorized access.");
        return;
    }
    await next();
});

app.Use(async (context, next) =>
{
    var isAuthenticated = context.Request.Query["authenticated"] == "true";
    if (!isAuthenticated)
    {
        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        await context.Response.WriteAsync("Access denied.");
        return;
    }
    context.Response.Cookies.Append("SecureCookie", "SecureData", new CookieOptions
    {
        HttpOnly = true,
        Secure = true
    });
    await next();
});

app.Use(async (context, next) =>
{
    await Task.Delay(100);
    await context.Response.WriteAsync("Processed asynchronously.");
    await next();
});

app.MapGet("/", () => "Hello World!");


static bool IsValidInput(string? input)
{
    return string.IsNullOrEmpty(input) || (input.All(char.IsLetterOrDigit) && !input.Contains("<scipt>"));
}

app.Run();
