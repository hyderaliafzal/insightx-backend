namespace ConnektaViz.API.DTOs.Response;

public class GraphResponseDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string DataSource { get; set; }
    public string MatricFunction { get; set; }
    public GraphTypeResponseDto GraphType { get; set; }
    public IEnumerable<GraphColumnResponseDto> GraphColumns { get; set; }
    public IEnumerable<SelectedTableColumnResponseDto> SelectedTableColumns { get; set; }
    public IEnumerable<GraphTableFilterResponseDto> GraphTableFilters { get; set; }
    public IEnumerable<GraphStylingResponseDto> GraphStyling { get; set; }
}
