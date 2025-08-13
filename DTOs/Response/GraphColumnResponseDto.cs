namespace ConnektaViz.API.DTOs.Response;

public class GraphColumnResponseDto
{
    public long Id { get; set; }
    public long GraphId { get; set; }
    public string Name { get; set; }
    public bool IsNumber { get; set; }
}
