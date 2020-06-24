using System;
using System.Collections.Generic;
using System.Text;

namespace BlocksCore.Infrastructure.Abstractions.Security.Permissions
{
    public class PermissionStereotype
    {
        public string Name { get; set; }
        public IEnumerable<Permission> Permissions { get; set; }

    }
}
