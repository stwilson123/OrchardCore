using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Navigation.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using OrchardCore.Navigation;

namespace BlocksCore.Navigation.Core
{
    public class DefaultUserNavigationManager : IUserNavigationManager
    {
        private readonly INavigationManager _navigationManager;
        private readonly IActionContextAccessor actionContext;

        public DefaultUserNavigationManager(INavigationManager navigationManager, IActionContextAccessor actionContext)
        {
            this._navigationManager = navigationManager;
            this.actionContext = actionContext;

        }
        public Task<IEnumerable<NavigationItem>> GetFilterMenuAsync(string name)
        {
            return _navigationManager.GetFilterMenuAsync(name, this.actionContext.ActionContext);
        }
    }
}
