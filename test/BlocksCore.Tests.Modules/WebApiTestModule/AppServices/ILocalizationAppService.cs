using System;
using System.Collections.Generic;
using System.Text;
using BlocksCore.Application.Abstratctions;

namespace WebApiTestModule.AppServices
{
    public interface ILocalizationAppService : IAppService
    {
        object GetLocalzedString();
    }
}
