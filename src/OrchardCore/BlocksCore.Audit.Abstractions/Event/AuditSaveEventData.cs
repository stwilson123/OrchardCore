using System;
using BlocksCore.Event.Abstractions;

namespace BlocksCore.Audit.Event
{
    public class AuditSaveEventData : EventData<AuditInfo>
    {
        public AuditSaveEventData(DateTime eventTime, object eventSource, AuditInfo entity) : base(eventTime, eventSource,entity)
        {
        }
    }
}
