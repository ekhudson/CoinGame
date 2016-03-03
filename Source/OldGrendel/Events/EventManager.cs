//#define DEBUG
using System;
using System.Collections.Generic;
using System.Reflection;

using UnityEngine;

public class EventManager : Singleton<EventManager> 
{
	public delegate void EventHandler<T>(object sender, T eventReceived) where T : EventBase;
	
	private abstract class EventPosterBase
	{
		public abstract bool IsEmpty { get; }
		public abstract void PostEvt(object sender, EventBase evt);
	}
	
	private class EventPoster<T> : EventPosterBase where T : EventBase
	{
		public event EventHandler<T> Listen = null;
		
		public override bool IsEmpty { get { return Listen == null; } }
		public override void PostEvt(object sender, EventBase evt)
		{
			if (Listen != null)
			{
				Listen(sender, (T)evt);	
			}
		}
	}
	
	private IDictionary<Type, EventPosterBase> _handlers = new Dictionary<Type, EventPosterBase>();
	
	protected override void Awake()
	{
		base.Awake();
	}

	public void AddHandler<T>(EventHandler<T> handler) where T : EventBase
	{
		EventPoster<T> poster;
		
		if (!_handlers.ContainsKey(typeof(T)))
		{
			poster = new EventPoster<T>();
			_handlers[typeof(T)] = poster;
		}
		else
		{
			poster = _handlers[typeof(T)] as EventPoster<T>;
		}
		
		poster.Listen += handler;
	}
	
	public void AddHandler(Type eventType, EventHandler<EventBase> handler)
	{
		AddRemoveHandler(eventType, handler, "AddHandler");
	}
	
	public void RemoveHandler<T>(EventHandler<T> handler) where T : EventBase
	{			
		if (!_handlers.ContainsKey(typeof(T)))
		{
			return;
		}
			
		EventPoster<T> poster = (EventPoster<T>)_handlers[typeof(T)];
		poster.Listen -= handler;
			
		if (_handlers[typeof(T)].IsEmpty)
		{
			_handlers.Remove(typeof(T));
		}
	}
		
	public void RemoveHandler(string eventTypeName, EventHandler<EventBase> handler)
	{
		Type eventType = GetEventType(eventTypeName);
		if (eventType != null)
		{
			RemoveHandler(eventType, handler);
		}
	}
	
	public void RemoveHandler(Type eventType, EventHandler<EventBase> handler)
	{
		AddRemoveHandler(eventType, handler, "RemoveHandler");
	}
	
	public void Post(Component sender, EventBase evt)
	{		
#if DEBUG
		Debug.Log(string.Format("Posting message from {0} : {1} @ {2} - {3}", sender.ToString(), evt.ToString(), evt.Place, evt.Time));
#endif		
		for (Type evtType = evt.GetType(); evtType != typeof(System.Object); evtType = evtType.BaseType)
		{
			if (_handlers != null && _handlers.ContainsKey(evtType))
			{
				if (_handlers[evtType] != null)
				{
					_handlers[evtType].PostEvt(sender, evt);
				}
				else
				{
				}				
			}
		}		
	}
	
	public void Post(EventBase evt)
	{
		Post(null, evt);
	}
	
	private Type GetEventType(string eventTypeName)
	{
		Type eventType = Type.GetType(eventTypeName, false);
		if (eventType == null)
		{
			Debug.LogError(string.Format("Looking up type {0}, but found none.", eventTypeName));
		}
		
		return eventType;
	}
	
	private void AddRemoveHandler(Type eventType, EventHandler<EventBase> handler, string addOrRemoveMethodName)
    {
        if (eventType == null)
        {
            Debug.LogError("error: can't add or remove a handler for null event type!");
            return;
        }

        if (!eventType.IsSubclassOf(typeof(EventBase)))
        {
            Debug.LogError(string.Format("error: {0} is not derived from EventBase, can't add or remove a handler for it", eventType.Name));
            return;
        }

        Type handlerType = typeof(EventManager.EventHandler<>).MakeGenericType(eventType);
        System.Delegate typedHandler = System.Delegate.CreateDelegate(handlerType, handler.Target, handler.Method);
        if (typedHandler == null)
        {
            Debug.LogError(string.Format("error: can't handle {0} with {1}", eventType.Name, handler.Method.Name));
            return;
        }

        // Find the generic AddHandler method.
        foreach (System.Reflection.MethodInfo mi in typeof(EventManager).GetMember(addOrRemoveMethodName, System.Reflection.MemberTypes.Method, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.InvokeMethod | System.Reflection.BindingFlags.Public))
        {
            if (mi.ContainsGenericParameters)
            {
                MethodInfo addHandlerMethod = mi.MakeGenericMethod(eventType);
                addHandlerMethod.Invoke(this, new object[] { typedHandler });
            }
        }
    }	
}
