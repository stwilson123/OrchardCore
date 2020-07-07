using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Localization;

namespace BlocksCore.Infrastructure.Abstractions.Security.Permissions
{
    public class Permission
    {

        public string ResourceKey { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        public string Category { get; set; }
        public IEnumerable<Permission> ImpliedBy { get; set; }

        public LocalizedString DisplayName { get; set; }

        public Permission(string name,string resourceKey)
        {
            Name = name;
            ResourceKey = resourceKey;
        }

    }
}
