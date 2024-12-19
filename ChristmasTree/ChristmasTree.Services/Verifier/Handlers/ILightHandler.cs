using System.Threading.Tasks;
using ChristmasTree.Data.Models;

namespace ChristmasTree.Services.Verifier.Handlers
{
    //Defines 2 methods
    public interface ILightHandler
    {
        // First moves from one handler to the next
        ILightHandler SetNext(ILightHandler handler);

        // Next checks if light has passed all checks
        Task<bool> HandleAsync(LightModel light);
    }
}