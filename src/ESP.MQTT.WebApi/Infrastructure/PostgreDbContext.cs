using Microsoft.EntityFrameworkCore;

namespace ESP.MQTT.WebApi.Infrastructure;

public class PostgreDbContext : DbContext
{
    public PostgreDbContext()
    { }

    public PostgreDbContext(DbContextOptions<PostgreDbContext> options) : base(options)
    { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PostgreDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}