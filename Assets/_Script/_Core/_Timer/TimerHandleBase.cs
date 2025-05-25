using System;
using UnityEngine;

public abstract class TimerHandleBase
{
    public float EndTime { get; private set; }
    public bool IsCompleted { get; private set; }

    private readonly UnityEngine.Object target;
    private Action onCompleteCallback;
    internal TimerHandleBase(UnityEngine.Object unityObject, float duration)
    {
        if (unityObject == null) throw new ArgumentNullException($"{unityObject} is null");

        target = unityObject;
        EndTime = duration + Time.time;
    }
    internal void Update()
    {
        if (IsCompleted) return;

        bool unityObjectDead = target == null;
        bool timeOut = Time.time > EndTime;

        bool shouldKill =
            unityObjectDead ||
            timeOut;

        bool onCompleteFire =
            timeOut;

        if (onCompleteFire)
        {
            if (onCompleteCallback != null) onCompleteCallback.Invoke();
        }

        if (shouldKill)
        {
            Kill();
        }
    }
    public void Kill()
    {
        Debug.Log("killed");
        onCompleteCallback = null;
        IsCompleted = true;
    }

}
