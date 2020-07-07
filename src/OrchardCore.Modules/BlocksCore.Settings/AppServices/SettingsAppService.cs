using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace BlocksCore.Settings.AppServices
{
    public class SettingsAppService : ISettingsAppService
    {
        [AllowAnonymous]
        public Task<object> Get(string groudId)
        {
            if(groudId == "DashboardRoute")
            { 
                return Task.FromResult<object>(new
                {
                    url = "/layout/AuthentionModule/dashboard",
                    title = "HOME_PAGE",
                    modifyPasswordPageUrl = "",
                    modifyPasswordPageName = "PASSWORD_PAGE",
                    copyRight = "Sys_CopyRight"
                });
            }
            else if(groudId == "getfactory")
            {
                return Task.FromResult<object>(new
                {
                    onoff = true,
                    list = new List<object>() {
                        new {
                            id = "1",
                            text = "工厂一",
                            isDefault = true,
                            path = "http://localhost:8080/Modules/Blocks.LayoutModule/dist/index.html#/"
                        },
                        new  {
                            id = "2",
                            text = "工厂二",
                            isDefault = false,
                            path = "http://localhost:8081/Modules/Blocks.LayoutModule/dist/index.html#/"
                        }
                    }
                });
            }

            return Task.FromResult<object>(new { });
        }
    }
}
