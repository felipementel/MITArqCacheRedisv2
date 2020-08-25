using Aplicacao.API.Controllers.Base;
using Aplicacao.Application.DTOs;
using Aplicacao.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Aplicacao.API.Controllers
{
    /// <summary>
    /// Product
    /// </summary>
    //[Authorize("Bearer")]
    [ApiVersion("1")]
    [Route("api/[controller]/{version}")]
    [ApiController]
    public class ProductsController : BaseController<ProductDTO, int>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="appService"></param>
        public ProductsController(IProductAppService appService) : base(appService) { }
    }
}
