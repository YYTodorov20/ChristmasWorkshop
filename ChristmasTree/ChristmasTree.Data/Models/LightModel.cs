using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ChristmasTree.Data.Models;

public class LightModel
{
    [Required]
    public int Id { get; set; }

    [Required]
    public int x { get; set; }

    [Required]
    public int y { get; set; }

    [Required]
    [Range(3F, 6F)]
    public float Radius { get; set; }

    [Required]
    required public string Color { get; set; }

    [Required]
    required public string Effects { get; set; }

    [Required]
    public string? Description { get; set; }

    [Required]
    public string? Ct { get; set; }

    [Required]
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;
}