using ChristmasTree.Data.Models;

namespace ChristmasTree.Services.Verifier.Handlers
{
    // This is the base class for other handlers
    public abstract class LightHandlerBase : ILightHandler
    {
        private ILightHandler? nextHandler; // saves ref for the current handler

        // prashta light-a kum drugia handler
        public ILightHandler SetNext(ILightHandler handler)
        {
            this.nextHandler = handler;
            return handler;
        }

        // Checks if there are other handlers
        // and if there aren't it returns the final result
        public virtual async Task<bool> HandleAsync(LightModel light)
        {
            return this.nextHandler == null || await this.nextHandler.HandleAsync(light); // Using 'this.' for clarity
        }
    }
}