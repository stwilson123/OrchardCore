using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Application.Abstratctions;

namespace BlocksCore.Localization.AppServices
{
    public interface ILocalizationAppService : IAppService
    {
        Task<object> Get();
    }
}
