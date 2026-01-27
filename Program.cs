var builder = WebApplication.CreateBuilder(args);

// Add a service for logging
builder.Services.AddHttpLogging((logging) =>
{
    logging.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.All;
    logging.RequestBodyLogLimit = 4096;
    logging.RequestBodyLogLimit = 4096;
});

// Add a service for authentication
builder.Services.AddAuthentication();

// Add a service for authorization
builder.Services.AddAuthorization();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("Home/Error");
}
else
{
    app.UseDeveloperExceptionPage();
}

// Authentication middleware
app.UseAuthentication();

// Authorization middleware
app.UseAuthorization();

// Http logging middleware
app.UseHttpLogging();

app.MapGet("/", () => "Hello, ASP.NET Core Middleware");

app.Use(async (context, next) =>
{
    Console.WriteLine($"Request Path: {context.Request.Path}");
    await next();
    Console.WriteLine($"Response Status Code: {context.Response.StatusCode}");
});

app.Use(async (context, next) =>
{
    var startTime = DateTime.UtcNow;
    Console.WriteLine($"Start time: {startTime}");
    await next();
    var duration = DateTime.UtcNow - startTime;
    Console.WriteLine($"Response time: {duration}");
});


app.Run();