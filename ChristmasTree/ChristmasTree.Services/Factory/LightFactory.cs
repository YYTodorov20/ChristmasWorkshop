using System.Text.Json;
using ChristmasTree.Data.Models;
using Microsoft.Extensions.Primitives;

namespace ChristmasTree.Services.Factory;
public class LightFactory
{
    private readonly Random random = new ();
    private readonly HttpClient httpClient;

    private readonly List<string> colors = new () { "blue-lt", "blue-dk", "red", "gold-lt", "gold-dk" };
    private readonly List<string> effects = new () { "g1", "g2", "g3" };

    public LightFactory(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<LightModel> CreateLight(string desc, StringValues christmasToken)
    {
        while (true)
        {
            // Generate random coordinates
            var (x, y) = this.GenerateCoordinatesWithinTriangle();

            // Validate coordinates with the external API
            if (await this.ValidateCoordinatesAsync(x, y))
            {
                return new LightModel
                {
                    x = (int)x,
                    y = (int)y,
                    Radius = (float)(this.random.NextDouble() * ((6 - 3) + 3)), // Random radius between 3 and 6
                    Color = this.colors[this.random.Next(this.colors.Count)],
                    Effects = this.effects[this.random.Next(this.effects.Count)],
                    Desc = desc,
                    Ct = christmasToken,
                };
            }
        }
    }

    private async Task<bool> ValidateCoordinatesAsync(double x, double y)
    {
        var response = await this.httpClient.GetAsync($"https://polygon.gsk567.com/?x={x}&y={y}");

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ValidationResponse>(content);
            return result?.IsValid ?? false;
        }

        return false;
    }

    private (double, double) GenerateCoordinatesWithinTriangle()
    {
        var r1 = this.random.NextDouble();
        var r2 = this.random.NextDouble();

        double x1 = 0.0, y1 = 170.3;
        double x2 = 125.8, y2 = 170.3;
        double x3 = 62.8, y3 = 14.9;

        double x = ((1 - Math.Sqrt(r1)) * x1) + ((Math.Sqrt(r1) * (1 - r2)) * x2) + ((Math.Sqrt(r1) * r2) * x3);
        double y = ((1 - Math.Sqrt(r1)) * y1) + ((Math.Sqrt(r1) * (1 - r2)) * y2) + ((Math.Sqrt(r1) * r2) * y3);

        return (x, y);
    }

    private class ValidationResponse
    {
        public bool IsValid { get; set; }
    }
}