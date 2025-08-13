namespace ConnektaViz.API.DTOs.Request;

public class RequestDto
{
    public int? Id { get; set; }
    public string Search { get; set; }
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
}
