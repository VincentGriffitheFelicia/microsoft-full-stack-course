var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IMyService, MyService>();

// Register HttpLogging
// builder.Services.AddHttpLogging(logging =>
// {
//     logging.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.All;
//     // You can customize which fields to log here
// });


// Add services to the container.
builder.Services.AddControllers();

var app = builder.Build();


app.Use(async (context, next) =>
{
    var myService = context.RequestServices.GetRequiredService<IMyService>();
    myService.LogCreation("First Middleware");
    await next.Invoke();
});

app.Use(async (context, next) =>
{
    var myService = context.RequestServices.GetRequiredService<IMyService>();
    myService.LogCreation("Second Middleware");
    await next.Invoke();
});

app.MapGet("/services", (IMyService myService) =>
{
    myService.LogCreation("Root.");
    return Results.Ok("Check the console for service creation logs.");
});

// app.UseHttpLogging();

// Configure the HTTP request pipeline.

// Remove HTTPS redirection so you can test with http

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


public interface IMyService
{
    void LogCreation(string message);
}

public class MyService : IMyService
{

    private readonly int _serviceId;

    public MyService()
    {
        _serviceId = new Random().Next(1000000, 9999999);
    }
    
    public void LogCreation(string message)
    {
        Console.WriteLine($"{message} - Service ID: {_serviceId}");
    }
}

