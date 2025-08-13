namespace ConnektaViz.API.DTOs.Request;

public class GraphRequestDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public int TypeId { get; set; }
    public string DataSource { get; set; }
    public string MatricFunction { get; set; }
    public IEnumerable<GraphColumnRequestDto> GraphColumns { get; set; }
    public IEnumerable<SelectedTableColumnRequestDto> SelectedTableColumns { get; set; }
    public IEnumerable<GraphTableFilterRequestDto> GraphTableFilters { get; set; }
    public IEnumerable<GraphStylingRequestDto> GraphStyling { get; set; }
}
