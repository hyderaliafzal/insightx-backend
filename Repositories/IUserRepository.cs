using ConnektaViz.API.Entities.Connekta;
namespace ConnektaViz.API.Repositories;

public interface IUserRepository
{
    Task<ResponseDto> GetUserByIdAsync(long id);
}
public class UserRepository(ConnektaDBContext _dbcontext, IMapper mapper, ILogger<UserRepository> logger) : IUserRepository
{
    public async Task<ResponseDto> GetUserByIdAsync(long id)
    {
        var response = new ResponseDto();
        try
        {
            User user = await _dbcontext.Users
                .Include(i => i.PermissionGroup)
                    .ThenInclude(t => t.Permissions)
            .FirstOrDefaultAsync(f => f.ID.Equals(id));


            logger.LogInformation($"GetUserByIdAsync Session --------> {user.LoginSession}");
            logger.LogInformation($"GetUserByIdAsync Current DateTime UTC --------> {DateTime.UtcNow}");
            if (user is null)
            {
                response.Success = false;
                response.Message = "User not found";
                response.Data = null;
            }
            else if (user.LoginSession.HasValue && user.LoginSession > DateTime.UtcNow)
            {
                response.Success = true;
                response.Message = "Request process successfully.";
                response.Data = mapper.Map<User, UserResponseDto>(user);
            }
            else
            {
                response.Success = false;
                response.Message = "User session is expired.";
                response.Data = null;
            }
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = "Error: " + ex.Message;
        }
        return response;
    }
}
