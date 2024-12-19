using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChristmasTree.Data.Models;
using ChristmasTree.Services.Factory;
using Microsoft.Extensions.Primitives;
using Microsoft.Identity.Client.Extensions.Msal;

namespace ChristmasTree.Services.Services
{
    public class LightService
    {
        private readonly List<LightModel> storage = new ();
        private readonly LightFactory lightFactory;
        private int idCounter = 1;

        public LightService(LightFactory lightFactory)
        {
            this.lightFactory = lightFactory;
        }

        public Task<LightModel> AddAsync(LightModel light)
        {
            light.Id = this.idCounter++;
            this.storage.Add(light);
            return Task.FromResult(light);
        }

        public Task<IEnumerable<LightModel>> GetAllAsync()
        {
            return Task.FromResult(this.storage.AsEnumerable());
        }

        public Task<LightModel?> GetByIdAsync(int id)
        {
            var result = this.storage.FirstOrDefault(l => l.Id == id);
            return Task.FromResult(result);
        }

        public Task<LightModel?> UpdateAsync(int id, LightModel updatedLight)
        {
            var existing = this.storage.FirstOrDefault(l => l.Id == id);
            if (existing == null)
            {
                return Task.FromResult<LightModel?>(null);
            }

            existing.x = updatedLight.x;
            existing.y = updatedLight.y;
            existing.Radius = updatedLight.Radius;
            existing.Color = updatedLight.Color;
            existing.Effects = updatedLight.Effects;
            existing.Desc = updatedLight.Desc;
            existing.Ct = updatedLight.Ct;

            return Task.FromResult((LightModel?)existing);
        }

        public Task<bool> DeleteAsync(int id)
        {
            var model = this.storage.FirstOrDefault(l => l.Id == id);
            if (model == null)
            {
                return Task.FromResult(false);
            }

            this.storage.Remove(model);
            return Task.FromResult(true);
        }
    }
}
