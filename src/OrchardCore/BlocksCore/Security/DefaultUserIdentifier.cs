using System;
using System.Collections.Generic;
using System.Text;
using BlocksCore.Abstractions.Security;

namespace BlocksCore.Security
{
    public class DefaultUserIdentifier : IUserIdentifier
    {
        public DefaultUserIdentifier(string tenantId,string userId,string userAccount,IEnumerable<string> roleIds)
        {
            TenantId = tenantId;
            UserId = userId;
            UserAccount = userAccount;
            RoleIds = roleIds;
        }
        public string TenantId { get; private set; }

        public string UserId { get; private set; }

        public string UserAccount { get; private set; }

        public IEnumerable<string> RoleIds { get; private set; }
    }
}
