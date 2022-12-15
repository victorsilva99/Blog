using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    [ApiController]
    [Route("")]
    public class HomeController : ControllerBase
    {
        // Health Check
        [HttpGet("")]
        public IActionResult Get()
        {
            return Ok( new
            {
                status = "Tudo ok!"
            });
        }
    }
}
