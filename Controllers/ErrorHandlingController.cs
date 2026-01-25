using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ErrorHandlingController : ControllerBase
{
    [HttpGet("division")]
    public IActionResult GetDivisionResult(int numerator, int denominator)
    {
        try
        {
            int result = numerator / denominator;
            return Ok("Here's the result: " + result);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error Message: ", ex.Message);
            Console.WriteLine("Error: Division by zero is not allowed.");
            throw;
        }
    }
}