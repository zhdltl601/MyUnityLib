using System;
using UnityEngine;

public class TimerHandle<T> : TimerHandleBase
    where T : UnityEngine.Object
{
    public new Action<T> OnCompleteCallback;
    private readonly T target;
    internal TimerHandle(T target, float duration)
        : base(target, duration)
    {
        if (target == null) throw new ArgumentNullException($"{target} is null");
        this.target = target;
    }
    protected override void OnEnd()
    {
        base.OnEnd();
        if(OnCompleteCallback != null)
        {
            OnCompleteCallback(target);
        }
    }
}
