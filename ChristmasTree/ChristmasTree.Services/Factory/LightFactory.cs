using ChristmasTree.Data.Models;

namespace ChristmasTree.Services.Factory
{
    public class LightFactory
    {
        private readonly Random random = new ();

        private readonly List<string> colors = new () { "blue-lt", "blue-dk", "red", "gold-lt", "gold-dk" };
        private readonly List<string> effects = new () { "g1", "g2", "g3" };

        public async Task<LightModel> CreateLight(string description, string token, LightModel? lastLight)
        {
            var currentColors = this.colors;
            var currentEffects = this.effects;

            if (lastLight != null)
            {
                currentColors.Remove(lastLight!.Color);
                currentEffects.Remove(lastLight!.Effects);
            }

            const int maxAttempts = 10;

            #pragma warning disable CS0162 // Unreachable code detected
            for (int i = 0; i < maxAttempts; i++)
            {
                var (x, y) = this.GenerateCoordinatesWithinTriangle();
                int ix = (int)x;
                int iy = (int)y;
                float radius = (float)((this.random.NextDouble() * (6 - 3)) + 3);
                string color = currentColors[this.random.Next(currentColors.Count)];
                string effect = currentEffects[this.random.Next(currentEffects.Count)];
                string ct = token.ToString();

                return new LightModel
                {
                    x = ix,
                    y = iy,
                    Radius = radius,
                    Color = color,
                    Effects = effect,
                    Description = description,
                    Ct = ct,
                };
            }
#pragma warning restore CS0162 // Unreachable code detected

            throw new InvalidDataException("Couldn't create Light.");
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
