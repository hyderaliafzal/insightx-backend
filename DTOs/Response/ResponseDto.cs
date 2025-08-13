namespace ConnektaViz.API.DTOs.Response;

public class ResponseDto
{
    public object Data { get; set; }
    public long Total { get; set; }
    public string Message { get; set; }
    public bool Success { get; set; }
}
