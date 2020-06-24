using System;
using System.Collections.Generic;
using System.Text;

namespace BlocksCore.Audit.Event
{
    public class AuditInfo
    {

        public string Parameters { get; set; }
        public string CustomData { get; set; }
        public string BrowserInfo { get; set; }
        public string ClientName { get; set; }
        public string ClientIpAddress { get; set; }
        public int ExecutionDuration { get; set; }
        public DateTime ExecutionTime { get; set; }
        public string OutParametersDescription { get; set; }
        public string OutParameters { get; set; }
        public string ParametersDescription { get; set; }
        public Exception SystemException { get; set; }
        public string MethodDescription { get; set; }
        public string MethodName { get; set; }
        public string ServiceName { get; set; }
        public int? ImpersonatorTenantId { get; set; }
        public long? ImpersonatorUserId { get; set; }
        public string UserAccount { get; set; }
        public long? UserId { get; set; }
        public int? TenantId { get; set; }
        public Exception Exception { get; set; }
    }
}
