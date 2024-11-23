using Microsoft.AspNetCore.Mvc;
using Shared;

namespace BackendServer.Controllers;

[ApiController]
[Route("api/default")]
public class DefaultController {
    [HttpGet]
    [Route("")]
    public ActionResult<Response> Get() {
        return new Response { ServerName = Server.Name };
    }
}