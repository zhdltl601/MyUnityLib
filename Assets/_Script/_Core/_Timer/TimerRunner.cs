using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.LowLevel;
using System;
using System.Collections.Generic;

public static class TimerRunner
{
    internal static class TimerUpdate
    {
        private readonly static List<TimerHandleBase> timers = new List<TimerHandleBase>(16);
        public static void AddTimer(TimerHandleBase timerHandle)
        {
            timers.Add(timerHandle);
        }
        public static void UpdateFunction()
        {
            for (int i = timers.Count - 1; i >= 0; i--)
            {
                TimerHandleBase timer = timers[i];
                timer.Update();
                if (timer.IsCompleted)
                {
                    timers.RemoveAt(i); // todo :
                }
            }
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void Init()
    {
        PlayerLoopSystem timerLoop = CustomPlayerLoop.CreateLoopSystem(typeof(TimerUpdate), new PlayerLoopSystem.UpdateFunction(TimerUpdate.UpdateFunction));
        CustomPlayerLoop.RegisterCustomLoop(typeof(Update), timerLoop);
    }
    /// <summary>
    /// creates and return Timer
    /// </summary>
    /// <typeparam name="T">callback argument</typeparam>
    /// <param name="target"></param>
    /// <param name="duration"></param>
    /// <returns>timer handle</returns>
    public static TimerHandle<T> Register<T>(T target, float duration)
        where T : UnityEngine.Object
    {
        TimerHandle<T> result = new TimerHandle<T>(target, duration);
        TimerUpdate.AddTimer(result);
        return result;
    }
    public static TimerHandle<T> Register<T>(T target, float duration, Action<T> callback)
        where T : UnityEngine.Object
    {
        TimerHandle<T> result = Register(target, duration);
        result.OnCompleteCallback = callback;
        return result;
    }
    public static TimerHandle<T> ReRegister<T>(TimerHandle<T> timerHandle, float duration)
        where T : UnityEngine.Object
    {
        TimerHandle<T> result = timerHandle;
        result.EndTime = duration;
        TimerUpdate.AddTimer(result);
        return result;
    }
    public static TimerHandle<T> ReRegister<T>(TimerHandle<T> timerHandle, float duration, Action<T> callback)
        where T : UnityEngine.Object
    {
        TimerHandle<T> result = ReRegister(timerHandle, duration);
        result.OnCompleteCallback = callback;
        return result;
    }

}