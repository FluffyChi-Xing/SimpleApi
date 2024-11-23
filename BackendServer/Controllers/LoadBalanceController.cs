using Microsoft.AspNetCore.Mvc;
using Shared;

namespace BackendServer.Controllers;

[ApiController]
[Route("api/loadbalance")]
public class LoadBalanceController {
    [HttpGet]
    [Route("")]
    public ActionResult<Response> Get() {
        return new Response { ServerName = Server.Name };
    }
}