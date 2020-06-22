using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OrchardCore.Security.Permissions;

namespace BlocksCore.Infrastructure.Abstractions.Security.Permissions
{
    public interface IPermissionProvider : OrchardCore.Security.Permissions.IPermissionProvider
    {
        Task<PermissionStereotype> GetStereotype(string name);
    }
}
