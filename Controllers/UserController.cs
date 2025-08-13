namespace ConnektaViz.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController(IUserRepository userService, IMemoryCache memoryCache) : Controller
{
    [HttpGet("Validate")]
    public async Task<ActionResult> Validate()
    {
        long.TryParse(User.Claims.FirstOrDefault(f => f.Type.Equals("UserId")).Value, out long userId);
        return Ok(await userService.GetUserByIdAsync(userId));
    }

    [HttpGet("LogoutHook")]
    public ActionResult LogoutHook()
    {
        long.TryParse(User.Claims.FirstOrDefault(f => f.Type.Equals("UserId")).Value, out long userId);
        memoryCache.Remove(userId);
        return Ok();
    }
}
