using Aplicacao.API.Controllers.Base;
using Aplicacao.Application.DTOs;
using Aplicacao.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Aplicacao.API.Controllers
{
    /// <summary>
    /// Customer
    /// </summary>
    //[Authorize("Bearer")]
    [ApiVersion("1")]
    [Route("api/[controller]/{version}")]
    [ApiController]
    public class CustomersController : BaseController<CustomerDTO, int>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="appService"></param>
        public CustomersController(ICustomerAppService appService) : base(appService) { }
    }
}
