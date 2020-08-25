using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Aplicacao.Application.Interfaces.Access;
using Aplicacao.Domain.Model.Access;

namespace Aplicacao.API.Controllers.Access
{
    [ApiVersion("1")]
    [Route("api/[controller]/{version}")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ILoginApplication _loginAppplication;

        public TokenController(ILoginApplication loginAppplication)
        {
            _loginAppplication = loginAppplication;
        }

        [HttpPost]
        [AllowAnonymous]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Token), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult Post([FromBody] User user)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var token = _loginAppplication.VerifyAccess(user);

            if (!token.Autenticated)
                return Forbid();

            return Ok(token);
        }
    }
}