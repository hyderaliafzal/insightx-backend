using ConnektaViz.API.Entities.Connekta;

namespace ConnektaViz.API.Database;

public class ConnektaDBContext(DbContextOptions<ConnektaDBContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<PermissionGroup> PermissionGroups { get; set; }
    public DbSet<Permission> Permissions { get; set; }
}