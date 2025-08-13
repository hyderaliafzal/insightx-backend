namespace ConnektaViz.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DataController(IDataRepository dataRepository,IWebHostEnvironment environment) : Controller
{

    [HttpGet("Sources")]
    public async Task<ActionResult> GetSourceNames()
    {
        var result = await dataRepository.GetSourceNames();
        return Ok(result);
    }

    [HttpGet("Properties")]
    public async Task<ActionResult> GetProperties(string dataSource)
    {
        var result = await dataRepository.GetPropertiesAsync(dataSource);
        return Ok(result);
    }

    [HttpPost("ForGraph")]
    public async Task<ActionResult> GetDataForGraph(GraphQueryDto request)
    {
        var result = await dataRepository.GetDataForGraph(request);
        return Ok(result);
    }

    [HttpPost("ForTable")]
    public async Task<ActionResult> GetDataForGraph(QueryDto request)
    {
        var result = await dataRepository.GetDataForTable(request);
        return Ok(result);
    }

    [HttpPost("GetMatricOperationValue")]
    public async Task<ActionResult> GetMatric(MatricRequestDto request)
    {
        var result = await dataRepository.GetMatric(request);
        return Ok(result);
    }

    [HttpPost("AsCsvFile")]
    public async Task<ActionResult> GetDataFileAsCsv(FileExportRequestDto request)
    {
        var result = await dataRepository.GetDataAsCsv(request);
        return PhysicalFile($"{environment.WebRootPath}/{result}", "application/octet-stream", $"{request.DataSource}.csv");
    }
}
