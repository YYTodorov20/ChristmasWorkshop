using System.Text.Json;
using ChristmasTree.Data.Models;
using Microsoft.Extensions.Primitives;

namespace ChristmasTree.Services.Factory
{
    public class LightFactory
    {
        private readonly Random random = new ();
        private readonly HttpClient httpClient;
        private readonly HashSet<(int x, int y, float radius, string color, string effects, string desc, string ct)> usedCombinations = new ();

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
                var (x, y) = this.GenerateCoordinatesWithinTriangle();
                int ix = (int)x;
                int iy = (int)y;
                float radius = (float)((this.random.NextDouble() * (6 - 3)) + 3);
                string color = this.colors[this.random.Next(this.colors.Count)];
                string effect = this.effects[this.random.Next(this.effects.Count)];
                string ct = christmasToken.ToString();

                var key = (x: ix, y: iy, radius: radius, color: color, effects: effect, desc: desc, ct: ct);

                if (!this.usedCombinations.Contains(key))
                {
                    this.usedCombinations.Add(key);
                    return new LightModel
                    {
                        x = ix,
                        y = iy,
                        Radius = radius,
                        Color = color,
                        Effects = effect,
                        Desc = desc,
                        Ct = ct,
                    };
                }
            }
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
    }
}
