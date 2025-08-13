namespace ConnektaViz.API.DTOs.Response;

public class GraphColumnDto
{
    public long Id { get; set; }
    public long GraphId { get; set; }

    public string Name { get; set; }
    public bool IsNumber { get; set; }
}
