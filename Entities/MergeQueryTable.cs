namespace ConnektaViz.API.Entities;

public class MergeQueryDetail
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("MergeQuery")]
    public int MergeQueryId { get; set; }
    public MergeQuery MergeQuery { get; set; }

    public string LeftTable { get; set; }
    public string LeftTableAlias { get; set; }
    public string JoinType { get; set; }
    public string RightTable { get; set; }
    public string RightTableAlias { get; set; }

    public string PrimaryColumn { get; set; }
    public string Operator { get; set; }
    public string ForeignColumn { get; set; }
}
