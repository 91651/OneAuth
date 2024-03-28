using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace Twos.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IIdentityServerInteractionService _interaction;

        public HomeController(ILogger<HomeController> logger, IIdentityServerInteractionService interaction)
        {
            _logger = logger;
            _interaction = interaction;
        }

        [HttpGet("error")]
        public async Task<IActionResult> Error(string errorId)
        {
            var message = await _interaction.GetErrorContextAsync(errorId);
            return Ok(message);
        }
    }
}