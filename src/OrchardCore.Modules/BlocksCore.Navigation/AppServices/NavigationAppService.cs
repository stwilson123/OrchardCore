using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Abstractions.Security;
using BlocksCore.Navigation.Abstractions;
using BlocksCore.Navigation.Core;
using BlocksCore.Navigation.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using OrchardCore.Navigation;

namespace BlocksCore.Navigation.AppServices
{
    public class NavigationAppService : INavigationAppService
    {
        private readonly IUserContext userContext;
        private readonly IUserNavigationManager navigationManager;
        private readonly IEnumerable<INavigationFilter> navigationFilters;

        public NavigationAppService(IUserNavigationManager navigationManager, IUserContext userContext, IEnumerable<INavigationFilter> navigationFilters)
        {
            this.navigationManager = navigationManager;
            this.userContext = userContext;
            this.navigationFilters = navigationFilters;
        }
        [Authorize]
        public async Task<IEnumerable<DTO.MenuItem>> GetCurrentUserNavigation(string name)
        {
            var menus = await this.navigationManager.GetFilterMenuAsync(name);

            foreach (var navigationFilter in navigationFilters)
            {
                menus = navigationFilter.OnFilterExecuting(menus);
            }

            return menus.Select(m => m.ToMenuItemDTO());
        }
    }
}
