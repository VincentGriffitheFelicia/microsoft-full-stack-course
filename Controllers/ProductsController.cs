using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

[Route("api/products")]
[ApiController]
public class ProductsController : ControllerBase
{

    private static List<Product> products = new List<Product>();


    // Retrieve all products
    [HttpGet]
    public ActionResult<List<Product>> GetAll()
    {
        return products;
    }


    // Get product by id
    [HttpGet("{id}")]
    public ActionResult<Product> GetProductById(int id)
    {
        Product? product = products.FirstOrDefault(p => p.Id == id);

        if (product != null)
        {
            return Ok(product);
        }
        else
        {
            return NotFound();
        }
    }



    // Add a new product
    [HttpPost]
    public ActionResult<Product> CreateProduct(Product newProduct)
    {
        newProduct.Id = products.Count + 1;
        products.Add(newProduct);
        return CreatedAtAction(nameof(GetProductById), new { id = newProduct.Id }, newProduct);
    }


    // Update a product by id
    [HttpPut("{id}")]
    public ActionResult<Product> UpdateProductById(int id, Product updatedProduct)
    {
        if (id > products.Count)
        {
            return NotFound("Product does not exist.");
        }
        
        products[id] = updatedProduct;
        return Ok(updatedProduct);
    }


    // Delete a product by id
    [HttpDelete("{id:int:min(0)}")]
    public ActionResult<string> Delete(int id)
    {
        if(id > products.Count)
        {
            return NotFound("Product does not exist.");
        }
        return Ok($"Deleted product with ID: {id}");
    }
}