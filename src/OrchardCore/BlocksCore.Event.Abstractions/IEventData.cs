using System;
using System.Collections.Generic;
using System.Text;

namespace BlocksCore.Event.Abstractions
{
    public interface IEventData
    {
        /// <summary>
        /// The time when the event occured.
        /// </summary>
        DateTime EventTime { get; }

        /// <summary>
        /// The object which triggers the event (optional).
        /// </summary>
        object EventSource { get; set; }
    }
}
