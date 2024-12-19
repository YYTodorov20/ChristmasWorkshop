using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChristmasTree.Data.Models;

namespace ChristmasTree.Services.Verifier
{
    // Malko komentari che si trqbva
    
    // Koe kak raboti?
    // P.S Koito editva posledno da mahne tiq neshta
    
    // Tova e za API-to ot zadanieto
    private class ApiResponse
    {
        public bool In { get; set; }
    }
    
    //Tozi interface definira 2 metoda
    public interface ILightHandler
    {
        // Purviq e za svurzvane i prehvurlqne ot edin handler na drug
        ILightHandler SetNext(ILightHandler handler); 
        // Vtoriq kazva dali light e minal checkovete ili ne
        Task<bool> HandleAsync(LightModel light);
    }
    
    // Tozi class e bazoviq za drugite handleri
    public abstract class LightHandlerBase : ILightHandler
    {
        private ILightHandler _nextHandler; // suhranqva referenciq kum sledvashtiq handler

        // prashta light-a kum drugia handler
        public ILightHandler SetNext(ILightHandler handler)
        {
            _nextHandler = handler;
            return handler;
        }
        
        // metod, koito checkva dali ima oshte handleri sled segashniq, i ako nqma
        // vrushta krainiq rezultat
        public virtual async Task<bool> HandleAsync(LightModel light)
        {
            return _nextHandler == null || await _nextHandler.HandleAsync(light);
        }
    }
    
    // Handler, koito checkva dali light-a e suzdaden v dadeniq interval (3 do 6)
    // kakto e dadeno v zadanieto
    public class RadiusHandler : LightHandlerBase
    {
        public override async Task<bool> HandleAsync(LightModel light)
        {
            if (light.Radius < 3F || light.Radius > 6F)
            {
                return false;
            }
            return await base.HandleAsync(light);
        }
    }
    
    // Dosadnata chast:
    
    //Handlera za koordinatite, toi checkva dali light-a e suzdaden v samiq triugulnik
    public class CoordinateHandler : LightHandlerBase
    {
        // Trite ni tochki, s koordinatite. Hardcodenati sa, zashtoto inache nqma da se spi
        private readonly (double, double) A = (0.00, 170.30);
        private readonly (double, double) B = (125.80, 170.30);
        private readonly (double, double) C = (62.80, 14.90);
        
        // Tui neshto e za API-to deto e v zadanieto
        private readonly HttpClient _httpClient;

        public CoordinateHandler(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
        // Malko matematika below
        
        // Tozi metod checkva dali light-a ni e v triugulnika.
        // Vzimam liceto na triugulnika i sled tova go sravnqvam s 3 po - malki triugulnika,
        // koito izpolzvat x i y na light modela ni
        private bool InTriangle(double x, double y)
        {
            double area = TriangleArea(A, B, C);
            double area1 = TriangleArea((x, y), B, C);
            double area2 = TriangleArea(A, (x, y), C);
            double area3 = TriangleArea(A, B, (x, y));
            return Math.Abs(area - (area1 + area2 + area3)) < 0.001;
        }
        
        // Tuka namiram liceto na triugulnika s dosadna formula po matematika
        private double TriangleArea((double, double) p1, (double, double) p2, (double, double) p3)
        {
            return Math.Abs((p1.Item1 * (p2.Item2 - p3.Item2) +
                             p2.Item1 * (p3.Item2 - p1.Item2) +
                             p3.Item1 * (p1.Item2 - p2.Item2)) / 2.0);
        }
        
        
        // Tuk re-write-vam metoda HandleAsync ot bazoviq klas
        public override async Task<bool> HandleAsync(LightModel light)
        {
            if (!InTriangle(light.x, light.y)) // check za dali tochite sa v triugulnika
            {
                return false;
            }
            
            // API check-a. Ako call-a kum API-to se provali, vrushtam false
            var response = await _httpClient.GetAsync($"https://polygon.gsk567.com/?x={light.x}&y={light.y}");
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            
            // Tuka purvoto chete responsa na API-to, a vtoroto pravi responsa ot JSON na C#
            // prez klasa ApiResponse
            var content = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(content);
            
            if (apiResponse?.In == true)
            {
                return false; // ako API-to kaje true, nachi koodrinatite sa hardcode-nati i grumim
            }
            return await base.HandleAsync(light);; // ako kaje false, nachi all good i produljavame
        }
        
        // Glavniq class
        public class LightHandler
        {
            // Storvam handlera, dali shte e purviq ili vtoriq
            private readonly ILightHandler _handlerChain;
            
            // Setvam chain-a ot handleri. Purvi e radiusa, sled tova sa coordinatite
            public LightHandler(HttpClient httpClient)
            {
                _handlerChain = new RadiusHandler();
                _handlerChain
                    .SetNext(new CoordinateHandler(httpClient))
            }
            
            // Izvikvam HandleAsync metoda.
            public async Task<bool> ProcessAsync(LightModel light)
            {
                return await _handlerChain.HandleAsync(light);
            }
        }
    }
}
