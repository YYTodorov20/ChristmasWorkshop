using System.ComponentModel.DataAnnotations;

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
    public string? Color { get; set; }

    [Required]
    public string? Effects { get; set; }

    [Required]
    public string? Desc { get; set; }

    [Required]
    public string? Ct { get; set; }
}