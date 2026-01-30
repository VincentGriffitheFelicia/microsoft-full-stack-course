var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var context = new CompanyContext();

var department = new Department { Name = "Engineering" };
context.Departments.Add(
    new Department
    {
        Name = "Engineering",
        Employees = new List<Employee>
        {
            new Employee
            {
                FirstName="Marissa",
                LastName="Johanssen",
                HireDate=DateTime.UtcNow
            },
            new Employee
            {
                FirstName="Ryan",
                LastName="Ford",
                HireDate=DateTime.UtcNow
            }
        }
    }
);

context.SaveChanges();

var employees = context.Employees.Where(emp => emp.Department.Name == "Engineering").ToList();
Console.WriteLine($"Employees in Engineering: {employees.Count}");

app.MapGet("/", () => "Hello World!");

app.Run();
