using System.Text.Json;

class Program
{
    public static void Main()
    {

        Person person = new Person { UserName = "John Doe", UserAge = 25 };
        string serialized = JsonSerializer.Serialize(person);

        Console.WriteLine(serialized);
    }

    class Person
    {
        required public string UserName { get; set; }
        public int? UserAge { get; set; }
    }
}