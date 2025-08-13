namespace ConnektaViz.API;

public class CustomAuthorizationHandler : AuthorizationHandler<IAuthorizationRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IAuthorizationRequirement requirement)
    {
        if (context.User.Claims.Any())
            context.Succeed(requirement);
        
        return Task.CompletedTask;
    }
}

