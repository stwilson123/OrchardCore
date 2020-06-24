using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlocksCore.Navigation.Abstractions
{
    public interface IUserNavigationManager
    {
        Task<IEnumerable<NavigationItem>> GetMenuAsync(string name);
    }
}
