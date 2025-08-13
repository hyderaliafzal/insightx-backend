namespace ConnektaViz.API.Entities;

public class Graph
{
    [Key]
    public long Id { get; set; }
    public string Name { get; set; }

    [ForeignKey("GraphType")]
    public int TypeId { get; set; }
    public virtual GraphType GraphType { get; set; }
    public string DataSource { get; set; }
    public string MatricFunction { get; set; }
    public virtual ICollection<GraphColumn> GraphColumns { get; set; } = [];
    public virtual ICollection<SelectedTableColumn> SelectedTableColumns { get; set; } = [];
    public virtual ICollection<GraphTableFilter> GraphTableFilters { get; set; } = [];
    public virtual ICollection<DashboardGraph> DashboardGraphs { get; set; } = [];
    public virtual ICollection<GraphStyling> GraphStyling { get; set; } = [];

}
