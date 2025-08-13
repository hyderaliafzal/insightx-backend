namespace ConnektaViz.API.DTOs.Response;

public class MergeQueryResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }

    public IEnumerable<MergeQueryDetailResponseDto> MergeQueryDetails { get; set; }
}
