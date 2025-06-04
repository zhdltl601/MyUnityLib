using System;
using UnityEngine;

public abstract class TimerHandleBase
{
    private float k_backingFieldEndtime;
    public float EndTime 
    {
        get => k_backingFieldEndtime; 
        internal set
        {
            k_backingFieldEndtime = value;
            IsCompleted = false;
        }
    }
    public bool IsCompleted { get; private set; }

    private readonly UnityEngine.Object target;
    public Action OnCompleteCallback; 
    internal TimerHandleBase(UnityEngine.Object unityObject, float duration)
    {
        if (unityObject == null) throw new ArgumentNullException($"{unityObject} is null");

        target = unityObject;
        EndTime = duration + Time.time;
    }
    internal void Update()
    {
        if (IsCompleted) return;

        bool targetDestroyed = target == null; //todo : this is expensive
        bool isTimeOut = Time.time > EndTime;

        bool shouldKill = targetDestroyed || isTimeOut;

        if (isTimeOut)
        {
            OnEnd();
        }

        if (shouldKill)
        {
            Kill();
        }
    }
    protected virtual void OnEnd()
    {
        if (OnCompleteCallback != null)
        {
            OnCompleteCallback.Invoke();
        }
    }
    public void Kill()
    {
        OnCompleteCallback = null;
        IsCompleted = true;
    }
}
