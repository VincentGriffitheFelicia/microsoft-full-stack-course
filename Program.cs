using System.Text;
using System.Text.Json;
using System.Xml.Serialization;

public class Person
{
    public string? UserName { get; set; }
    public int UserAge { get; set; }
}


public class Program
{
    public static void Main()
    {
        Person person = new Person
        {
            UserName = "John Doe",
            UserAge = 25
        };

        
        Console.WriteLine("==========BINARY SERIALIZATION==========");
        using (FileStream fs = new FileStream("person.dat", FileMode.Create))
        {
            Console.WriteLine("Serializing...");
            BinaryWriter binaryWriter = new BinaryWriter(fs);
            binaryWriter.Write(person.UserName);
            binaryWriter.Write(person.UserAge);
            Console.WriteLine("Binary Serialization Complete.");
        }

        Console.WriteLine("==========XML SERIALIZATION==========");
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(Person));
        using (StreamWriter streamWriter = new StreamWriter("person.xml"))
        {
            Console.WriteLine("Serializing...");
            xmlSerializer.Serialize(streamWriter, person);
            Console.WriteLine("XML Serialization Complete.");
        }

        Console.WriteLine("==========JSON SERIALIZATION==========");
        string serializedPerson = JsonSerializer.Serialize(person);
        using (FileStream fileStream = new FileStream("person.json", FileMode.Create))
        {
            byte[] bytes = Encoding.UTF8.GetBytes(serializedPerson);
            fileStream.Write(bytes);
        }

    }
}