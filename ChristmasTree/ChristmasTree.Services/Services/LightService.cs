using ChristmasTree.Data;
using ChristmasTree.Data.Models;
using ChristmasTree.Services.Factory;
using ChristmasTree.Services.Models;
using Microsoft.EntityFrameworkCore;

namespace ChristmasTree.Services.Services
{
    public class LightService
    {
        private readonly EntityContext entityContext;
        private readonly LightFactory lightFactory;
        private readonly TokenService tokenService;

        public LightService(EntityContext context, LightFactory lightFactory, TokenService tokenService)
        {
            this.entityContext = context;
            this.lightFactory = lightFactory;
            this.tokenService = tokenService;
        }

        public async Task AddAsync(string description, string token)
        {
            await this.tokenService.DeleteTokens(token);

            var lastLight = await this.entityContext.LightModels.OrderByDescending(entityContext => entityContext.CreationDate).FirstOrDefaultAsync();

            // validator here

            var newLight = await this.lightFactory.CreateLight(description, token, lastLight);

            this.entityContext.LightModels.Add(newLight);
            await this.entityContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<LightModelDTO>> GetAllAsync()
        {
            var lights = await this.entityContext.LightModels.ToListAsync();
            return lights.Select(light => new LightModelDTO
            {
                Id = light.Id,
                x = light.x,
                y = light.y,
                Radius = light.Radius,
                Color = light.Color,
                Effects = light.Effects,
                Description = light.Description,
                Ct = light.Ct,
            });
        }
    }
}
