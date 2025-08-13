namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GraphController(IGraphRepository graphRepository) : ControllerBase
{
    [HttpGet("All")]
    public ActionResult All()
    {
        var result = graphRepository.GetAll();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(long id)
    {
        var result = await graphRepository.GetById(id);
        return Ok(result);
    }

    [HttpGet("Types")]
    public ActionResult GetTypes()
    {
        var result = graphRepository.GetGraphTypes();
        return Ok(result);
    }

    [HttpPost("Save")]
    public async Task<ActionResult> SaveAsync(GraphRequestDto requestDto)
    {
        var result = await graphRepository.SaveAsync(requestDto);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(long id)
    {
        var result = await graphRepository.DeleteAsync(id);
        return Ok(result);
    }
}
