namespace ConnektaViz.API.DTOs.Request;

public class FileExportRequestDto
{
    public string DataSource { get; set; }
    public IEnumerable<FilterDto> Filters { get; set; }
}
