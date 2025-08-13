namespace ConnektaViz.API.Entities;

public class MergeQuery
{
    public MergeQuery()
    {
        MergeQueryDetails = new HashSet<MergeQueryDetail>();
    }
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }

    public virtual ICollection<MergeQueryDetail> MergeQueryDetails { get; set; }
}
