namespace ConnektaViz.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DataMergeController(IDataMergeRepository dataMergeRepository) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> GetAsync([FromQuery] RequestDto request)
    {
        var result = await dataMergeRepository.GetAsync(request);
        return Ok(result);
    }

    [HttpPost("Save")]
    public async Task<ActionResult> SaveAsync(MergeQueryRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Name) || !request.MergeQueryDetails.Any())
            return Ok(new ResponseDto { Success = false, Message = "Bad request payload." });

        if (await dataMergeRepository.IsDuplicate(request))
            return Conflict($"Record is already exists with the name of {request.Name}");

        var result = await dataMergeRepository.SaveAsync(request);
        return Ok(result);
    }

    [HttpDelete("MergeQuery/{id}")]
    public async Task<ActionResult> DeleteMergeQueryAsync(int id)
    {
        var result = await dataMergeRepository.Delete(id);
        return Ok(result);
    }
}
