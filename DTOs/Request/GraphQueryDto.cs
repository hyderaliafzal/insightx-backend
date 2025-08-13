namespace ConnektaViz.API.DTOs.Request;

public class GraphQueryDto
{
    public string DataSource { get; set; }
    public string XColumn { get; set; }
    public string YColumn { get; set; }

    public string MatricFunction { get; set; }
    public IEnumerable<FilterDto> Filters { get; set; }
}
