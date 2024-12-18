using ChristmasTree.Models;
using Microsoft.EntityFrameworkCore;

namespace ChristmasTree.Data;

public class EntityContext : DbContext
{
    public EntityContext(DbContextOptions<EntityContext> options)
        : base(options)
    {
        
    }

    public DbSet<LightModel> LightModels { get; set; }
}