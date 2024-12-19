using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChristmasTree.Services.Models
{
    public class LightModelDTO
    {
        [Required]
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [Required]
        [JsonPropertyName("x")]
        public int x { get; set; }

        [Required]
        [JsonPropertyName("y")]
        public int y { get; set; }

        [Required]
        [Range(3F, 6F)]
        [JsonPropertyName("radius")]
        public float Radius { get; set; }

        [Required]
        [JsonPropertyName("color")]
        required public string Color { get; set; }

        [Required]
        [JsonPropertyName("effects")]
        required public string Effects { get; set; }

        [Required]
        [JsonPropertyName("desc")]
        public string? Description { get; set; }

        [Required]
        [JsonPropertyName("ct")]
        public string? Ct { get; set; }
    }
}
