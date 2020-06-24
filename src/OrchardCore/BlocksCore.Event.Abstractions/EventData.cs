using System;
using System.Collections.Generic;
using System.Text;

namespace BlocksCore.Event.Abstractions
{
    [Serializable]
    public class EventData<TData> : EventData
    {
        public EventData(DateTime eventTime, object eventSource,TData entity) : base(eventTime, eventSource)
        {
            Data = entity;
        }

        public EventData(DateTime eventTime, TData entity) : this(eventTime, null, entity)
        {
            
        }

        public EventData(TData entity) : this(default(DateTime), entity)
        {

        }
        /// <summary>
        /// Related entity with this event.
        /// </summary>
        public TData Data { get; private set; }
 
    }

    public abstract class EventData : IEventData
    {
        public DateTime EventTime { get; set; }

        public object EventSource { get; set; }

        public EventData(DateTime eventTime, object eventSource)
        {
            EventSource = eventSource;
            EventTime = eventTime;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="entity">Related entity with this event</param>
        public EventData(DateTime eventTime) : this(eventTime, null)
        {
        }
        public EventData() 
        {
        }
    }
}
