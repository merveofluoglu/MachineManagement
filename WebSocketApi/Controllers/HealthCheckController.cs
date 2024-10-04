using Microsoft.AspNetCore.Mvc;

namespace WebSocketApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthCheckController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Server is running");
        }
    }
}
