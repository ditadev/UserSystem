using Microsoft.AspNetCore.Mvc;

namespace UserSystem.Api.Controllers;

public abstract class AbstractController : ControllerBase
{
    protected ulong GetContextUserId()
    {
        return ulong.Parse(HttpContext.User.Claims.First(i => i.Type == "sub").Value);
    }
}