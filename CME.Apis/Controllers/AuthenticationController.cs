using CME.Business.Interfaces;
using CME.Business.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TSoft.Framework.ApiUtils.Controllers;

namespace CME.Apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ApiControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService titleService)
        {
            _authenticationService = titleService;
        }
        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponseModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Login(LoginRequestModel requestModel)
        {
            return await ExecuteFunction(async () =>
            {
                return await _authenticationService.Login(requestModel.Username, requestModel.Password,requestModel.MacAddress);
            });
        }
    }
}
