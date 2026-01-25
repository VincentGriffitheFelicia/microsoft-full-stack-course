using System.Text.Json;
using System.Xml.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});

var app = builder.Build();

Person person = new Person { UserName="John Doe", UserAge=25 };

app.MapGet("/", () =>
{
    string json = JsonSerializer.Serialize(person);
    return TypedResults.Text(json, "application/json");
});

app.MapGet("/json", () =>
{
    return TypedResults.Json(person);
});

app.MapGet("/auto", () =>
{
    return person;
});


app.MapGet("/custom-serializer", () =>
{
    var options = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseUpper
    };

    string json = JsonSerializer.Serialize(person, options);
    return TypedResults.Text(json, "application/json");
});


app.MapGet("/xml", () =>
{
    XmlSerializer xmlSerializer = new XmlSerializer(typeof(Person));
    StringWriter stringWriter = new StringWriter();
    xmlSerializer.Serialize(stringWriter, person);
    var xmlOutput = stringWriter.ToString();
    return TypedResults.Text(xmlOutput, "application/xml");
});

app.Run();


public class Person
{
    public string? UserName { get; set; }
    public int UserAge { get; set; }
}