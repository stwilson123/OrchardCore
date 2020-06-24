using System;
using System.Collections.Generic;
using System.Text;

namespace BlocksCore.Event.Abstractions
{
    public interface IDistributedEventHandler
    {
    }

    public interface IDistributedEventHandler<in TEventData> : IDistributedEventHandler where TEventData : IEventData
    {
        /// <summary>
        /// Handler handles the event by implementing this method.
        /// </summary>
        /// <param name="eventData">Event data</param>
        void HandleEvent(TEventData eventData);
    }

}
