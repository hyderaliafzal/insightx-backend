namespace ConnektaViz.API.Entities;

public class GraphTableFilter
{
    [Key]
    public long Id { get; set; }

    [ForeignKey("Graph")]
    public long GraphId { get; set; }
    public virtual Graph Graph { get; set; }

    public string Field { get; set; }
    public string Operator { get; set; }
    public string Keyword { get; set; }
    public string DisplayOperator { get; set; }
}
