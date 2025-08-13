namespace ConnektaViz.API.Entities;

public class SelectedTableColumn
{
    [Key]
    public long Id { get; set; }

    [ForeignKey("Graph")]
    public long GraphId { get; set; }
    public virtual Graph Graph { get; set; }

    public string Name { get; set; }
}
