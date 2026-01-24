using Newtonsoft.Json;

public class Product
{
    public string? Name { get; set; }
    public decimal Price { get; set; }
    public List<string>? Tags { get; set; }
}

public class Program
{
    public static void Main()
    {
        string json = @"{
            ""Name"": ""Laptop"",
            ""Price"": 1299.99,
            ""Tags"": [""Electronics"", ""Computers"", ""Portable""]
        }";

        Product? product = JsonConvert.DeserializeObject<Product>(json);

        if (product != null)
        {
            Console.WriteLine($"Product: {product.Name}, Price: {product.Price}, Tags: {string.Join(", ", product.Tags ?? new List<string>())}");
        }

        Console.WriteLine("=======================================================");

        Product newProduct = new Product
        {
            Name = "Smartphone",
            Price = 699.99m,
            Tags = new List<string> { "Electronics", "Mobile" },
        };

        string serializedJson = JsonConvert.SerializeObject(newProduct, Formatting.Indented);
        Console.WriteLine($"Serialized JSON: \n{serializedJson}");

    }
}