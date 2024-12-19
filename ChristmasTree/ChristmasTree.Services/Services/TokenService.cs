using ChristmasTree.Data;
using Microsoft.EntityFrameworkCore;

namespace ChristmasTree.Services.Services
{
    public class TokenService
    {
        private readonly EntityContext entityContext;

        public TokenService(EntityContext entityContext)
        {
            this.entityContext = entityContext;
        }

        public async Task DeleteTokens(string token)
        {
            var oldLights = await this.entityContext.LightModels
                .Where(l => l.Ct != token)
                .ToListAsync();

            await this.entityContext.SaveChangesAsync();
        }
    }
}
