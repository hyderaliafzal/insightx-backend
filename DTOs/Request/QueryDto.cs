namespace ConnektaViz.API.DTOs.Request;

public class QueryDto
{
    public string DataSource { get; set; }
    public long PageSize { get; set; }
    public long PageNumber { get; set; }
    public string Sort { get; set; }
    public string SortOrder { get; set; }
    public IList<FilterDto> Filters { get; set; }
}
