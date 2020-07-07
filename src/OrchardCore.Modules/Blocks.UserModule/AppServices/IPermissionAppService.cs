using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Application.Abstratctions;

namespace BlocksCore.Users.AppServices
{
    public interface IPermissionAppService : IAppService
    {
        public Task<IEnumerable<string>> Get();
    }
}
