using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovableObjectController : MonoBehaviour
{
    [System.Serializable]
    public class MovableObjectEvent
    {
        public EventTransceiver.Events OnEvent;
        public MonoBehaviour FromObject;
    }

    [System.Serializable]
    public class MovableObjectPosition
    {
        public Vector3 Position;
        public Quaternion Rotation;
    }

    public MovableObject TargetObject;
    public List<MovableObjectEvent> MovableObjectEvents = new List<MovableObjectEvent>();
    public float MoveSpeed;
    [HideInInspector]public List<MovableObjectPosition> Positions = new List<MovableObjectPosition>();
    public bool Interruptable = true;
    public MovableObject.MoveModes MoveMode;
    public MovableObject.LoopModes LoopMode;

    private Vector3 mTargetOriginalPosition = Vector3.zero;
    private Vector3 mTargetOriginalSize = Vector3.zero;
    private Quaternion mTargetOriginalRotation = Quaternion.identity;

    private Dictionary<System.Type, List<MovableObjectEvent>> mEventDictionary = new Dictionary<System.Type, List<MovableObjectEvent>>();

    private void Start()
    {
        if (MovableObjectEvents.Count <= 0)
        {
            return;
        }

        foreach(MovableObjectEvent evt in MovableObjectEvents)
        {
            EventManager.Instance.AddHandler(EventTransceiver.LookupEvent(evt.OnEvent).GetType(), EventHandler);

            if (mEventDictionary.ContainsKey(EventTransceiver.LookupEvent(evt.OnEvent).GetType()))
            {
                mEventDictionary[EventTransceiver.LookupEvent(evt.OnEvent).GetType()].Add(evt);
            }
            else
            {
                mEventDictionary.Add(EventTransceiver.LookupEvent(evt.OnEvent).GetType(), new List<MovableObjectEvent>(){evt});
            }
        }
    }

    public void EventHandler(object sender, EventBase evt)
    {
        foreach(MovableObjectEvent movEvt in mEventDictionary[evt.GetType()])
        {
            if (movEvt.FromObject != null && sender as MonoBehaviour != movEvt.FromObject)
            {
                continue;
            }

            TargetObject.GivePath(new List<MovableObjectPosition>(Positions), MoveSpeed, Interruptable, MoveMode, LoopMode);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (Application.isEditor && !Application.isPlaying)
        {
            mTargetOriginalPosition = TargetObject.transform.position;
            mTargetOriginalSize = TargetObject.gameObject.GetComponent<Renderer>().bounds.size;
            mTargetOriginalRotation = TargetObject.transform.rotation;
        }

        foreach(MovableObjectPosition position in Positions)
        {
            Gizmos.DrawWireCube(mTargetOriginalPosition + position.Position, position.Rotation * (mTargetOriginalRotation * mTargetOriginalSize));
        }
    }

}
