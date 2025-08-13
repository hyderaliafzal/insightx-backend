namespace ConnektaViz.API.DTOs.Request;

public class MatricRequestDto
{
    public string DataSource { get; set; }
    public string BindingColumn { get; set; }
    public string MatricFunction { get; set; }
    public IList<FilterDto> Filters { get; set; }
}
