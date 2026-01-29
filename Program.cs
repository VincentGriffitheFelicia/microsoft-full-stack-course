using System.Collections.Concurrent;
using Course_Repository.Services;
using Course_Repository.Middleware;
using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



// Add CORS if needed
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure middleware in correct order:
// 1. Exception Handling (first - catches all exceptions)
app.UseMiddleware<ExceptionHandlingMiddleware>();

// 2. Authentication (next - validates tokens)
app.UseMiddleware<AuthenticationMiddleware>();

// 3. Logging (last - logs all requests/responses after other middleware)
app.UseMiddleware<RequestLoggingMiddleware>();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

// Welcome endpoint
app.MapGet("/", () => new { message = "User Management API", version = "1.0" });

app.Run();
