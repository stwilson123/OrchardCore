using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BlocksCore.Infrastructure.Abstractions.Security.Permissions;
using Microsoft.Extensions.Caching.Memory;
using OrchardCore.Modules;

namespace BlocksCore.Infrastructure.Security.Permissions
{
    public class DefaultPermissionManager : IPermissionManager
    {
        private readonly IEnumerable<Abstractions.Security.Permissions.IPermissionProvider> _permissionProviders;
        private readonly IMemoryCache _cache;
        private const string CacheKeyPrefix = "PermissionDictionary-";

        public DefaultPermissionManager(IEnumerable<Abstractions.Security.Permissions.IPermissionProvider> permissionProviders, IMemoryCache cache)
        {
            this._permissionProviders = permissionProviders;
            this._cache = cache;
        }
        public async Task<IDictionary<string, Abstractions.Security.Permissions.Permission>> GetPermissionsAsync(string permissionStereotypeName)
        {
            
           var cachedDictionary = await _cache.GetOrCreateAsync(CacheKeyPrefix + permissionStereotypeName, k => Task.Factory.StartNew<IDictionary<string, Permission>>((t) =>
            {
                IList<PermissionStereotype> permissionStereotype = new List<PermissionStereotype>();
                foreach (var provider in _permissionProviders)
                {
                    permissionStereotype.Add(provider.GetStereotype(permissionStereotypeName).Result);
                }
                return permissionStereotype.Where(permissions => permissions != null).SelectMany(permissionStereotype => permissionStereotype?.Permissions?.Where(p => p.Name != null)).ToDictionary(p => p.Name.ToLower(), p => p);
            }, LazyThreadSafetyMode.ExecutionAndPublication));
            return cachedDictionary;
        }

        //public Task<IEnumerable<Permission>> GetPermissionsAsync()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
