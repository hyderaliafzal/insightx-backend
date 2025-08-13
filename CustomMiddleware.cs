namespace ConnektaViz.API;

public class LoginSessionMiddleware(ConnektaDBContext dBContext, IMemoryCache memoryCache, ILogger<LoginSessionMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context.User.Identity.IsAuthenticated)
        {
            long.TryParse(context.User.Claims.FirstOrDefault(f => f.Type.Equals("UserId"))?.Value, out long userId);
            if (userId == 0)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Invalid User ID.");
                return;
            }

            if (!memoryCache.TryGetValue(userId, out DateTime? session) || session <= DateTime.UtcNow)
            {
                var user = await dBContext.Users.FindAsync(userId);
                if (user?.LoginSession > DateTime.UtcNow)
                {
                    memoryCache.Set(user.ID, user.LoginSession);
                    session = user.LoginSession;
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("User session is expired.");
                    return;
                }
            }

            // Proceed with the request if session is valid
            await next.Invoke(context);
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("User not authenticated.");
        }
    }
}
