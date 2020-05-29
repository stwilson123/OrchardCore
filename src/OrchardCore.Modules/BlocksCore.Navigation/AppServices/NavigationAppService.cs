using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Abstractions.Security;
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
        private readonly INavigationManager navigationManager;
        private readonly IActionContextAccessor actionContext;
        private readonly IUserContext userContext;

        public NavigationAppService(INavigationManager navigationManager, IActionContextAccessor actionContext,IUserContext userContext)
        {
            this.navigationManager = navigationManager;
            this.actionContext = actionContext;
            this.userContext = userContext;
        }
        [Authorize]
        public async Task<IEnumerable<DTO.MenuItem>> GetCurrentUserNavigation(string name)
        {
            var currentUser = userContext.GetCurrentUser();
           var menus = await this.navigationManager.GetFilterMenuAsync(name, this.actionContext.ActionContext);

            return menus.Select(m => m.ToMenuItemDTO());
        }
    }
}
