using System.Net.Http;
using System.Threading.Tasks;
using ChristmasTree.Data.Models;

namespace ChristmasTree.Services.Verifier.Handlers
{
    // Main class
    public class LightHandler
    {
        // Store whatever handler
        private readonly ILightHandler handlerChain;

        // Set the chain of handlers
        public LightHandler(HttpClient httpClient)
        {
            this.handlerChain = new RadiusHandler();
            this.handlerChain.SetNext(new CoordinateHandler(httpClient));
        }

        // Call HandleAsync method
        public async Task<bool> ProcessAsync(LightModel light)
        {
            return await this.handlerChain.HandleAsync(light);
        }
    }
}