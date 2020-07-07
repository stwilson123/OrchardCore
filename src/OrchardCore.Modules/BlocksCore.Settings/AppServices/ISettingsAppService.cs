using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Application.Abstratctions;

namespace BlocksCore.Settings.AppServices
{
    public interface ISettingsAppService : IAppService
    {
        Task<object> Get(string groupId);
    }
}
