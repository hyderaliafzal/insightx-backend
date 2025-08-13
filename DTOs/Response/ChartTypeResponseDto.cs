namespace ConnektaViz.API.DTOs.Response;

public class GraphTypeResponseDto
{
    public int Id { get; set; }
    public string Type { get; set; }
    public string Label { get; set; }
    public string Icon { get; set; }
    public bool IsActive { get; set; }
}
