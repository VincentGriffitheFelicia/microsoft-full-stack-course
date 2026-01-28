
using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var blogs = new List<Blog>
{
    new Blog { Title="My first post", Body="This is my first post"},
    new Blog { Title="My second post", Body="This is my second post"}
};

if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "Hello World!").ExcludeFromDescription();

app.MapGet("/blogs", () =>
{
    return blogs;
});

app.MapGet("/blogs/{id}", Results<Ok<Blog>, NotFound> (int id) =>
{
    if (id < 0 || id > blogs.Count - 1)
    {
        return TypedResults.NotFound();
    }
    else
    {
        return TypedResults.Ok(blogs[id]);
    }
})
.WithSummary("Get a single blog by ID")
.WithDescription("Returns a single blog based on the provided ID.");

app.Run();
