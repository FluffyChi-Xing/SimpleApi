using Microsoft.AspNetCore.Mvc;
using Shared;

namespace BackendServer.Controllers;

[ApiController]
[Route("api/uarewrite")]
public class UARewriteController : ControllerBase {
    [HttpGet]
    [Route("")]
    public ActionResult<Response> Get() {
        return new Response {
            ServerName = Server.Name, UserAgent = Request.Headers["User-Agent"]
        };
    }
}