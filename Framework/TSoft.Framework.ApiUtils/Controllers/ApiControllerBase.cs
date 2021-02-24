using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TSoft.Framework.ApiUtils.Controllers
{
    public class ApiControllerBase : ControllerBase
    {
        protected async Task<IActionResult> ExecuteFunction<T>(string permission, Func<string, Task<T>> func)
        {
            var result = await func(permission);
            return new OkObjectResult(new GenericResult((object)result, true, "success!!!"));
        }

        protected async Task<IActionResult> ExecuteFunction<T>(Func<Task<T>> func)
        {
            try
            {
                var result = await func();
                return new OkObjectResult(new GenericResult(result, true, "success!!!"));
            }
            catch (Exception ex)
            {
                return new OkObjectResult(new GenericResult(false, ex.InnerException != null ? ex.InnerException.Message : ex.Message));
            }
        }
    }
}
