
using (var context = new ApplicationDbContext())
{
    context.Database.EnsureCreated();

    // CREATE
    context.Products.Add(
        new Product
        {
            Name = "Laptop",
            Price = 999.99m
        }
    );
    context.Products.Add(
        new Product
        {
            Name = "Phone",
            Price = 649.99m
        }
    );
    context.SaveChanges();

    // READ
    Console.WriteLine("Products in the database:");
    var products = context.Products.ToList();
    foreach (var product in products)
    {
        Console.WriteLine($"Product: {product.Name}, Price: {product.Price}");
    }

    // UPDATE
    var productToUpdate = context.Products.First();
    productToUpdate.Price = 1200.99m;
    context.SaveChanges();

    // DELETE
    var productToDelete = context.Products.OrderByDescending(p => p.ProductID).Last();
    context.Products.Remove(productToDelete);
    context.SaveChanges();
}