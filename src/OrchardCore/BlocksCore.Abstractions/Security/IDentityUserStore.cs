using System;
using System.Collections.Generic;
using System.Text;

namespace BlocksCore.Abstractions.Security
{
    public interface IDentityUserStore
    {
        IEnumerable<string> GetRoles(string userId);
    }
}
