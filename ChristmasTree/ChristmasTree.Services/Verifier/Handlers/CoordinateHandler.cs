using System.Net.Http;
using System.Threading.Tasks;
using ChristmasTree.Data.Models;
using ChristmasTree.Services.Verifier.API;
using Newtonsoft.Json;

namespace ChristmasTree.Services.Verifier.Handlers
{
    //Checks if light is created within the area of the triangle
    public class CoordinateHandler : LightHandlerBase
    {
        // Trite ni tochki, s koordinatite. Hardcodenati sa, zashtoto inache nqma da se spi
        private readonly (double, double) a = (0.00, 170.30);
        private readonly (double, double) b = (125.80, 170.30);
        private readonly (double, double) c = (62.80, 14.90);

        // Tui neshto e za API-to deto e v zadanieto
        private readonly HttpClient? httpClient;

        public CoordinateHandler(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        // Here I rewrite HandleAsync from base class
        public override async Task<bool> HandleAsync(LightModel light)
        {
            // checks if the dots are in the triangle
            if (!this.InTriangle(light.x, light.y))
            {
                return false;
            }

            // API check-a. Ako call-a kum API-to se provali, vrushtam false
            if (this.httpClient != null)
            {
                var response = await this.httpClient.GetAsync($"https://polygon.gsk567.com/?x={light.x}&y={light.y}");
                if (!response.IsSuccessStatusCode)
                {
                    return false;
                }

                // First checks the response of the API, the second transforms it from JSON to C#
                // using the ApiResponse class
                var content = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(content);

                if (apiResponse?.In == true)
                {
                    return false; // if API says true, then the code is bad
                }
            }

            return await base.HandleAsync(light); // if it says false, we are good to go
        }

        // Math time

        // This method checks if light is in the triangle
        // Gets the area of the triangle, then checks it with 3 other smaller triangles
        // which use the light model's x and y
        //Rider gives errors if i don't use this. every time
        private bool InTriangle(double x, double y)
        {
            double area = this.TriangleArea(this.a, this.b, this.c);
            double area1 = this.TriangleArea((x, y), this.b, this.c);
            double area2 = this.TriangleArea(this.a, (x, y), this.c);
            double area3 = this.TriangleArea(this.a, this.b, (x, y));
            return Math.Abs(area - (area1 + area2 + area3)) < 0.001;
        }

        // Finds the area of the triangle using MATHS
        private double TriangleArea((double, double) p1, (double, double) p2, (double, double) p3)
        {
            return Math.Abs(
                ((p1.Item1 * (p2.Item2 - p3.Item2)) +
                 (p2.Item1 * (p3.Item2 - p1.Item2)) +
                 (p3.Item1 * (p1.Item2 - p2.Item2))) / 2.0);
        }
    }
}
