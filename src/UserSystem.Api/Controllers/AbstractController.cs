using Microsoft.AspNetCore.Mvc;

namespace UserSystem.Api.Controllers;

public abstract class AbstractController : ControllerBase
{
    protected long GetContextUserId()
    {
        return long.Parse(HttpContext.User.Claims.First(i => i.Type == "sub").Value);
    }
}