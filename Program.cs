var builder = WebApplication.CreateBuilder(args);

// Register HttpLogging
builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.All;
    // You can customize which fields to log here
});


// Add services to the container.
builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpLogging();

// Configure the HTTP request pipeline.

// Remove HTTPS redirection so you can test with http

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();