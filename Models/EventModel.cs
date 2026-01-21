using System;
using System.ComponentModel.DataAnnotations;

public class EventModel
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public DateTime Date { get; set; }
    [Required]
    public string Location { get; set; } = string.Empty;
    [Required]
    public string Description { get; set; } = string.Empty;
}
