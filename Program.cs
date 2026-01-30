
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

var context = new HRDbContext();

var departments = context.Departments.ToList();
var employees = context.Employees.ToList();
var employeesWithDepartment = context.Employees.Include(e => e.Department);

var options = new JsonSerializerOptions
{
    ReferenceHandler = ReferenceHandler.Preserve,
    WriteIndented = true
};

Console.WriteLine("=================DEPARTMENTS===============");
foreach (var dep in departments)
{
    var json = JsonSerializer.Serialize(dep, options);
    Console.WriteLine(json);
}

Console.WriteLine("=================EMPLOYEES===============");
foreach (var emp in employees)
{
    var json = JsonSerializer.Serialize(emp, options);
    Console.WriteLine(json);
}

Console.WriteLine("=================EMPLOYEES WITH DEPARTMENT===============");
foreach (var emp in employeesWithDepartment)
{
    var json = JsonSerializer.Serialize(emp, options);
    Console.WriteLine(json);
}