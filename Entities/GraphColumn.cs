namespace ConnektaViz.API.Entities;

public class GraphColumn
{
    [Key]
    public long Id { get; set; }

    [ForeignKey("Graph")]
    public long GraphId { get; set; }
    public virtual Graph Graph { get; set; }

    public string Name { get; set; }
    public bool IsNumber { get; set; }
}
