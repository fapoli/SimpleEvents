using System;
using System.Collections.Generic;
using System.Linq;

namespace MoodyLib.SimpleEvents {

    /// <summary>
    /// A simple event dispatcher that allows for subscribing and dispatching events.
    /// </summary>
    public static class EventDispatcher {

        private static readonly Dictionary<Type, List<Action<SimpleEvent>>> _eventListeners = new();
        private static readonly Dictionary<object, Action<SimpleEvent>> _handlerReferences = new();
        
        /// <summary>
        /// Subscribes to an event.
        /// </summary>
        /// <param name="handler">The handler that is going to be called when the event is dispatched.</param>
        /// <typeparam name="T">The type of event that is going to be subscribed to. Must be a subclass of SimpleEvent.</typeparam>
        public static void Subscribe<T>(Action<T> handler) where T : SimpleEvent {
            if (!_eventListeners.ContainsKey(typeof(T))) {
                _eventListeners.Add(typeof(T), new List<Action<SimpleEvent>>());
            }
            
            if (_handlerReferences.ContainsKey(handler)) return;

            Action<SimpleEvent> action = e => handler((T)e);
            _handlerReferences[handler] = action;
            _eventListeners[typeof(T)].Add(action);
        }

        /// <summary>
        /// Unsubscribes from an event.
        /// </summary>
        /// <param name="handler">The handler that is going to be unsubscribed from the event.</param>
        public static void Unsubscribe<T>(Action<T> handler) where T : SimpleEvent {
            if (!_eventListeners.ContainsKey(typeof(T))) return;
            var action = _handlerReferences[handler];
            if (action!= null && _eventListeners[typeof(T)].Contains(action)) {
                _eventListeners[typeof(T)].Remove(action);
            }
        }

        /// <summary>
        /// Dispatches an event to all listeners.
        /// </summary>
        /// <param name="e">The event that is going to be dispatched.</param>
        public static void Dispatch(SimpleEvent e) {
            if (!_eventListeners.ContainsKey(e.GetType())) return;
            
            foreach (var handler in _eventListeners[e.GetType()].ToList()) {
                try {
                    handler(e);
                }catch (Exception ex) {
                    Unsubscribe(handler);
                }
            }
        }

    }
}