using MyClientNamespace;
public class Program
{
    public static async Task Main(string[] args)
    {
        var httpClient = new HttpClient();
        var client = new MyClientNamespace.OctoberApiClient("http://localhost:5000", httpClient);
        var user = await client.UserAsync(1);
        Console.WriteLine(user);
    }
}