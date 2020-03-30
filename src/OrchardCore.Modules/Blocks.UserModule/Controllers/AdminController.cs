using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Blocks.UserModule.Controllers
{
    //[Route("[area]/[controller]/[action]")]
    [Route("[Area]/[controller]/[action]")]
    [Area("cc/c")]
    public class AdminController : ControllerBase
    {
        private ILogger<AdminController> logger;

        public AdminController(ILogger<AdminController> logger)
        {
            this.logger = logger;
        }

        [IgnoreAntiforgeryToken]
        public void GetUserName()
        {
            logger.LogInformation("GetUserName");
        }
    }
}
