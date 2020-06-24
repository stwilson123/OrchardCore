using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlocksCore.Infrastructure.Abstractions.Security.Permissions;
using BlocksCore.Infrastructure.Security.Permissions;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Xunit;

namespace BlocksCore.Test.Security.Permissions
{
    public class PermissionManagerTest
    {
        Mock<Infrastructure.Abstractions.Security.Permissions.IPermissionProvider> _permissionProvider;
        IMemoryCache _memoryCache;

        static IEnumerable<Permission> emptyPermission = Enumerable.Empty<Permission>();
        static IEnumerable<Permission> singlePermission = new List<Permission>() { new Permission("add"), new Permission("index") } ;

        public PermissionManagerTest()
        {
            _permissionProvider = new Mock<Infrastructure.Abstractions.Security.Permissions.IPermissionProvider>();
            _permissionProvider.Setup(p => p.GetStereotype(It.Is<string>(t => t == "admin")))
                .ReturnsAsync(new PermissionStereotype() { Name = "admin", Permissions = singlePermission });
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
        }

        [Fact]
        public async void GetPermissionsFromStereotypeName()
        {
            var permissionManager = new DefaultPermissionManager(new[] { _permissionProvider.Object }, _memoryCache);

            var adminPermissions = await permissionManager.GetPermissionsAsync("admin");
            Assert.True(adminPermissions.Count == 2);
            Assert.True(adminPermissions.ContainsKey("add"));
            Assert.True(adminPermissions.ContainsKey("index"));

            var notFoundPermissions = await permissionManager.GetPermissionsAsync("notfound");
            Assert.True(notFoundPermissions.Count == 0);
            
        }
    }
}
