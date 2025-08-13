namespace ConnektaViz.API.DTOs.Request;

public class MergeQueryRequestDto
{
    public int Id { get; set; }
    public string Name { get; set; }

    public IList<MergeQueryDetailRequestDto> MergeQueryDetails { get; set; }
}
