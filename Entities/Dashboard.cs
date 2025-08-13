namespace ConnektaViz.API.Entities;
public class Dashboard
{
    public Dashboard()
    {
        DashboardGraphs = new HashSet<DashboardGraph>();
    }
    [Key]
    public long Id { get; set; }

    public required string Name { get; set; }
    public required int SortOrder { get; set; }
    public virtual ICollection<DashboardGraph> DashboardGraphs { get; set; }
}
