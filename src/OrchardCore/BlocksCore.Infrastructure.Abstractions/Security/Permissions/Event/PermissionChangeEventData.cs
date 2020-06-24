using System;
using System.Collections.Generic;
using System.Text;
using BlocksCore.Event.Abstractions;

namespace BlocksCore.Infrastructure.Abstractions.Security.Permissions.Event
{
    public class PermissionChangeEventData : EventData
    {
        public PermissionChangeEventData(DateTime eventTime) : base(eventTime)
        {
        }

        public string RoleId { get; }

        public PermissionChangeEventData(string roleId) : base()
        {
            RoleId = roleId;
        }
    }
}
