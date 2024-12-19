using System.Net.Http;
using System.Threading.Tasks;
using ChristmasTree.Data.Models;
using ChristmasTree.Services.Verifier.Handlers;

namespace ChristmasTree.Services.Verifier
{
    // Main file for verification
    public class LightVerifier
    {
        private readonly LightHandler lightHandler;
        public LightVerifier(HttpClient httpClient)
        {
            this.lightHandler = new LightHandler(httpClient);
        }

        // Method to call when verifying light.
        // Passes the task of verification towards the LightHandler and then trough ProcessAsync will go towards the rest of the handlers
        public async Task<bool> VerifyLightAsync(LightModel light)
        {
            return await this.lightHandler.ProcessAsync(light);
        }
    }
}