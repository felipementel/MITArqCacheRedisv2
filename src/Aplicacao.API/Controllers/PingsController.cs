using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aplicacao.API.Controllers
{
    [AllowAnonymous]
    [ApiVersion("1")]
    [Route("api/[controller]/{version}")]
    public class PingsController : Controller
    {
        /// <summary>
        /// Action para testes de disponibilidade. 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get() => Ok("Pong");
    }
}