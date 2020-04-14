using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApiTestModule
{
    //[Route("[area]/[controller]/[action]")]
    [ApiController]
    [Route("[area]/[controller]/[action]")]
    public class AdminController : ControllerBase
    {
        private ILogger<AdminController> logger;

        public AdminController(ILogger<AdminController> logger)
        {
            this.logger = logger;
        }

        [IgnoreAntiforgeryToken]
        public object DefaultMethodFromObject(dynamic inputParams )
        {
           // logger.LogInformation("GetUserName");
            return inputParams;
        }
    }
}
