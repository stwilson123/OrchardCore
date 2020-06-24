using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BlocksCore.Event.Abstractions;
using Microsoft.Extensions.Logging;
using BlocksCore.SyntacticAbstractions.Extensions;
using System.Reflection;

namespace BlocksCore.Event.Core
{
    public class EventBus : IEventBus
    {
        /// <summary>
        /// All registered handler factories.
        /// Key: Type of the event
        /// Value: List of handler factories
        /// </summary>
        private readonly ConcurrentDictionary<Type, List<IEventHandlerFactory>> _handlerFactories;
        private readonly ILogger _logger;


        /// <summary>
        /// Creates a new <see cref="EventBus"/> instance.
        /// Instead of creating a new instace, you can use <see cref="Default"/> to use Global <see cref="EventBus"/>.
        /// </summary>
        public EventBus(ILogger<EventBus> logger)
        {
            _handlerFactories = new ConcurrentDictionary<Type, List<IEventHandlerFactory>>();
            this._logger = logger;
        }
 
        public void Register<TEventData>(IEventHandlerFactory handlerFactory) where TEventData : IEventData
        {
             Register(typeof(TEventData), handlerFactory);
        }

        public void Register(Type eventType, IEventHandlerFactory handlerFactory)
        {
            var factories = GetOrCreateHandlerFactories(eventType);
            factories.Add(handlerFactory);

        }

        public void Trigger<TEventData>(TEventData eventData) where TEventData : IEventData
        {
            Trigger((object)null, eventData);
        }

        public void Trigger<TEventData>(object eventSource, TEventData eventData) where TEventData : IEventData
        {
            Trigger(typeof(TEventData), eventSource, eventData);
        }

        public void Trigger(Type eventType, IEventData eventData)
        {
            Trigger(eventType, null, eventData);
        }

        public void Trigger(Type eventType, object eventSource, IEventData eventData)
        {
            var exceptions = new List<Exception>();

            TriggerHandlingException(eventType, eventSource, eventData, exceptions);

            if (exceptions.Any())
            {
                if (exceptions.Count == 1)
                {
                    exceptions[0].ReThrow();
                }

                throw new AggregateException(
                    "More than one error has occurred while triggering the event: " + eventType, exceptions);
            }
        }

        public Task TriggerAsync<TEventData>(TEventData eventData) where TEventData : IEventData
        {
            return TriggerAsync((object)null, eventData);
        }

        public Task TriggerAsync<TEventData>(object eventSource, TEventData eventData) where TEventData : IEventData
        {
            return TriggerAsync(typeof(TEventData), eventSource, eventData);
        }

        public Task TriggerAsync(Type eventType, IEventData eventData)
        {
            return TriggerAsync(eventType, null, eventData);
        }

        public Task TriggerAsync(Type eventType, object eventSource, IEventData eventData)
        {
            ExecutionContext.SuppressFlow();

            var task = Task.Factory.StartNew(
                () =>
                {
                    try
                    {
                        Trigger(eventType, eventSource, eventData);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex.ToString(), ex);
                    }
                });

            ExecutionContext.RestoreFlow();

            return task;
        }

        public void Unregister(Type eventType, IEventHandler handler)
        {
            throw new NotImplementedException();
        }

        public void Unregister<TEventData>(IEventHandlerFactory factory) where TEventData : IEventData
        {
            throw new NotImplementedException();
        }

        public void Unregister(Type eventType, IEventHandlerFactory factory)
        {
            throw new NotImplementedException();
        }

        public void UnregisterAll<TEventData>() where TEventData : IEventData
        {
            throw new NotImplementedException();
        }

        public void UnregisterAll(Type eventType)
        {
            throw new NotImplementedException();
        }

        private List<IEventHandlerFactory> GetOrCreateHandlerFactories(Type eventType)
        {
            return _handlerFactories.GetOrAdd(eventType, (type) => new List<IEventHandlerFactory>());
        }

        private void TriggerHandlingException(Type eventType, object eventSource, IEventData eventData,
           List<Exception> exceptions)
        {
            //TODO: This method can be optimized by adding all possibilities to a dictionary.

            eventData.EventSource = eventSource;

            foreach (var handlerFactories in GetHandlerFactories(eventType))
            {
                foreach (var handlerFactory in handlerFactories.EventHandlerFactories)
                {
                    var eventHandler = handlerFactory.GetHandler();
                    try
                    {
                        if (eventHandler == null)
                        {
                            throw new Exception(
                                $"Registered event handler for event type {handlerFactories.EventType.Name} does not implement IEventHandler<{handlerFactories.EventType.Name}> interface!");
                        }

                        //((IEventHandler<IEventData>)eventHandler).HandleEvent(eventData);
                        //TOLDO test exec direct
                        //                        var handlerType = typeof(IEventHandler<>).MakeGenericType(handlerFactories.EventType);
                        //                        var method = handlerType.GetMethod(
                        //                            "HandleEvent",
                        //                            new[] { handlerFactories.EventType }
                        //                        );
                        //                        method.Invoke(eventHandler, new object[] { eventData });

                        var method = eventHandler.GetType()
                            .GetMethod("HandleEvent", new[] { handlerFactories.EventType });
                        method.Invoke(eventHandler, new object[] { eventData });
                    }
                    catch (TargetInvocationException ex)
                    {
                        exceptions.Add(ex.InnerException);
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(ex);
                    }
                    finally
                    {
                        //TODO
                        //handlerFactory.ReleaseHandler(eventHandler);
                    }
                }
            }

            ////Implements generic argument inheritance. See IEventDataWithInheritableGenericArgument
            //if (eventType.GetTypeInfo().IsGenericType &&
            //    eventType.GetGenericArguments().Length == 1 &&
            //    typeof(IEventDataWithInheritableGenericArgument).IsAssignableFrom(eventType))
            //{
            //    var genericArg = eventType.GetGenericArguments()[0];
            //    var baseArg = genericArg.GetTypeInfo().BaseType;
            //    if (baseArg != null)
            //    {
            //        var baseEventType = eventType.GetGenericTypeDefinition().MakeGenericType(baseArg);
            //        var constructorArgs = ((IEventDataWithInheritableGenericArgument)eventData).GetConstructorArgs();
            //        var baseEventData = (IEventData)Activator.CreateInstance(baseEventType, constructorArgs);
            //        baseEventData.EventTime = eventData.EventTime;
            //        Trigger(baseEventType, eventData.EventSource, baseEventData);
            //    }
            //}
        }

        private IEnumerable<EventTypeWithEventHandlerFactories> GetHandlerFactories(Type eventType)
        {
            var handlerFactoryList = new List<EventTypeWithEventHandlerFactories>();

            foreach (var handlerFactory in _handlerFactories.Where(
                hf => ShouldTriggerEventForHandler(eventType, hf.Key)))
            {
                handlerFactoryList.Add(
                    new EventTypeWithEventHandlerFactories(handlerFactory.Key, handlerFactory.Value));
            }

            return handlerFactoryList.ToArray();
        }

        private class EventTypeWithEventHandlerFactories
        {
            public Type EventType { get; }

            public List<IEventHandlerFactory> EventHandlerFactories { get; }

            public EventTypeWithEventHandlerFactories(Type eventType, List<IEventHandlerFactory> eventHandlerFactories)
            {
                EventType = eventType;
                EventHandlerFactories = eventHandlerFactories;
            }
        }

        private static bool ShouldTriggerEventForHandler(Type eventType, Type handlerType)
        {
            //Should trigger same type
            if (handlerType == eventType)
            {
                return true;
            }

            //Should trigger for inherited types
            if (handlerType.IsAssignableFrom(eventType))
            {
                return true;
            }

            return false;
        }

    }
}
