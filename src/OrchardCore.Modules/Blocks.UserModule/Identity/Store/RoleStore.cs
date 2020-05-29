using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using OrchardCore.Security;

namespace BlocksCore.Users.Identity.Store
{
    public class RoleStore : IRoleClaimStore<IRole>, IQueryableRoleStore<IRole>
    {
        public IQueryable<IRole> Roles => throw new NotImplementedException();

        public Task AddClaimAsync(IRole role, Claim claim, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> CreateAsync(IRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> DeleteAsync(IRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
           
        }

        public Task<IRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Claim>> GetClaimsAsync(IRole role, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetNormalizedRoleNameAsync(IRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetRoleIdAsync(IRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetRoleNameAsync(IRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task RemoveClaimAsync(IRole role, Claim claim, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task SetNormalizedRoleNameAsync(IRole role, string normalizedName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetRoleNameAsync(IRole role, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> UpdateAsync(IRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
