using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ConnektaViz.API.Database;

public class ConnektaVizContext(DbContextOptions<ConnektaVizContext> options) : DbContext(options)
{
    public DbSet<GraphType> GraphType { get; set; }
    public DbSet<Graph> Graph { get; set; }
    public DbSet<GraphColumn> GraphColumn { get; set; }
    public DbSet<SelectedTableColumn> SelectedTableColumn { get; set; }

    [NotMapped]
    public DbSet<KeyValue> KeyValue { get; set; }
    public DbSet<Dashboard> Dashboard { get; set; }
    public DbSet<DashboardGraph> DashboardGraph { get; set; }

    public DbSet<MergeQuery> MergeQuery { get; set; }
    public DbSet<MergeQueryDetail> MergeQueryDetail { get; set; }

    public DbSet<TableVW> Table { get; set; }
    public DbSet<ColumnSP> Column { get; set; }

    public DbSet<GraphStyling> GraphStyling { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<GraphType>().HasData(
           new GraphType { Id = 1, Type = "bar", Label = "Bar Graph", Icon = "assets/images/icons/bar.svg", IsActive = true },
           new GraphType { Id = 2, Type = "line", Label = "Line Graph", Icon = "assets/images/icons/line.svg", IsActive = true },
           new GraphType { Id = 3, Type = "pie", Label = "Pie Graph", Icon = "assets/images/icons/pie.svg", IsActive = true },
           new GraphType { Id = 4, Type = "doughnut", Label = "Doughnut Graph", Icon = "assets/images/icons/doughnut.svg", IsActive = true },
           new GraphType { Id = 5, Type = "scatter", Label = "Scatter Plot", Icon = "assets/images/icons/scatter.svg", IsActive = true },
           new GraphType { Id = 6, Type = "score", Label = "Score Card", Icon = "assets/images/icons/scatter.svg", IsActive = true }
       );

        modelBuilder.Entity<TableVW>()
                   .ToView("vu_Tables")
                   .HasNoKey();

        modelBuilder.Entity<ColumnSP>()
            .HasNoKey();

        modelBuilder.Entity<KeyValue>()
            .HasNoKey();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.ConfigureWarnings(warnings =>
               warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
    }
}
