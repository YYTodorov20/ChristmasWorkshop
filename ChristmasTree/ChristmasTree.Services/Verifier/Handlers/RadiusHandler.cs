using System.Threading.Tasks;
using ChristmasTree.Data.Models;

namespace ChristmasTree.Services.Verifier.Handlers
{
    // Handler, which checks if light is created in the interval (3 do 6)
    // as it is said in the zadanie
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
}