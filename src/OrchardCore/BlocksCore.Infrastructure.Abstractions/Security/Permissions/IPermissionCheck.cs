using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Abstractions.Security;

namespace BlocksCore.Infrastructure.Abstractions.Security.Permissions
{
    public interface IPermissionChecker
    {
        Task<bool> IsGrantedAsync(IUserIdentifier user, Permission permission);
    }
}
