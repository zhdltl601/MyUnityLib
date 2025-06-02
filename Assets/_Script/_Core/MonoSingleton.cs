using System;
using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    protected virtual MonoSingletonFlags SingletonFlag { get; }
    private static T _instance = null;

    private static bool IsShuttingDown { get; set; }
    public static T Instance
    {
        get
        {
            if (_instance is null) //using C# null check
            {
                if (IsShuttingDown) return null;

                //if (singletonFlag.HasFlag(MonoSingletonFlags.SingletonPreset)) _instance = GetPresetSingleton();
                else _instance = RuntimeInitialize();
            }
            return _instance;
        }
    }
    private static T GetPresetSingleton() => default;
    private static T RuntimeInitialize()
    {
        string singletonMessage = "Runtime_Singleton" + typeof(T).Name;
        GameObject gameObject = new GameObject(name: singletonMessage);
        T result = gameObject.AddComponent<T>();
        Debug.LogWarning(singletonMessage, gameObject);
        return result;
    }
    protected virtual void Awake()
    {
        //check two singleton error
        if (_instance is not null)
        {
            Destroy(gameObject);
            throw new Exception("TwoSingletons_" + typeof(T).Name);
        }

        //custom singleton attribute setting
        if (SingletonFlag.HasFlag(MonoSingletonFlags.DontDestroyOnLoad)) DontDestroyOnLoad(gameObject);
        if (SingletonFlag.HasFlag(MonoSingletonFlags.Hide)) gameObject.hideFlags = HideFlags.HideInHierarchy;

#if UNITY_EDITOR
        Debug.Log($"[Singleton_Awake] [type : {typeof(T).Name}] [name : {gameObject.name}]");
#endif
        _instance = this as T;
    }
    protected virtual void OnDestroy()
    {
        if (_instance == this) _instance = null; //explicitly setting null for C# null check
    }
    protected virtual void OnApplicationQuit()
    {
        IsShuttingDown = true;
    }
}