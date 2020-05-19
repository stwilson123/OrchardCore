using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Application.Abstratctions;

namespace BlocksCore.Navigation.AppServices
{
    public  interface INavigationAppService : IAppService
    {
          Task<IEnumerable<DTO.MenuItem>> GetCurrentUserNavigation(string name);
    }
}
