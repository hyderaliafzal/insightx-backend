namespace ConnektaViz.API.DTOs.Request;

public class GraphColumnRequestDto
{
    public long Id { get; set; }
    public long GraphId { get; set; }
    public string Name { get; set; }
    public bool IsNumber { get; set; }
}
