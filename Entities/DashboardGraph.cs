namespace ConnektaViz.API.Entities;

public class DashboardGraph
{
    [Key]
    public long Id { get; set; }

    public required double X { get; set; }

    public required double Y { get; set; }

    public required double Height { get; set; }
    public required double Width { get; set; }

    [ForeignKey("Dashboard")]
    public long DashboardId { get; set; }
    public Dashboard Dashboard { get; set; }

    [ForeignKey("Graph")]
    public long GraphId { get; set; }
    public Graph Graph { get; set; }
}
