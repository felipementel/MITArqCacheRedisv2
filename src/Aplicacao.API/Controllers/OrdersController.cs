using Aplicacao.API.Controllers.Base;
using Aplicacao.Application.DTOs;
using Aplicacao.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Aplicacao.API.Controllers
{
    /// <summary>
    /// Orders
    /// </summary>
    //[Authorize("Bearer")]
    [ApiVersion("1")]
    [Route("api/[controller]/{version}")]
    [ApiController]
    public class OrdersController : BaseController<OrderDTO, int>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="appService"></param>
        public OrdersController(IOrderAppService appService) : base(appService) { }

        [ApiExplorerSettings(IgnoreApi = true)]
        public override Task<IActionResult> Delete(int id)
        {
            return base.Delete(id);
        }
    }
}
