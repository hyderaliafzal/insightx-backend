namespace ConnektaViz.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DashboardController(IDashboardRepository dashboardRepository, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var response = await dashboardRepository.GetAllAsync();
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> SaveDashboard(DashboardRequestDto requestDto)
    {
        var result = await dashboardRepository.SaveAsync(requestDto);
        return Ok(result);
    }

    [HttpPost("UpdateSortOrder")]
    public async Task<IActionResult> UpdateSortOrder(List<DashboardRequestDto> requestDto)
    {
        var result = await dashboardRepository.UpdateSortOrderAsync(requestDto);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        var dashboard = await dashboardRepository.GetByIdAsync(id);
        if (dashboard == null)
            return NotFound();

        return Ok(dashboard);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await dashboardRepository.DeleteAsync(id);

        return Ok(response);
    }
}
