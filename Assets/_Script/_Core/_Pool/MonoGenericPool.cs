using UnityEngine;

namespace Custom.Pool
{
    public static class MonoGenericPool<T>
        where T : MonoBehaviour, IPoolable
    {
        private static bool canInit = true;
        
        private static MonoPool<T> monoPool;
        /// <summary>
        /// This function must be called before calling any other methods.
        /// </summary>
        public static void Initialize(PoolPrefabMonoBehaviourSO prefabSO)
        {
            if(canInit == false)return;
            
            canInit = false;
            
            bool isMonoPoolNotInitialized = monoPool == null;
            Debug.Assert(isMonoPoolNotInitialized, $"field:monoPool is already initialized. {prefabSO.name}");

            T prefab = prefabSO.GetMono as T;
            monoPool = new MonoPool<T>(prefab);
            
            Debug.Assert(prefab != null, "failed to cast Mono to T, Wrong PoolPrefabMonoBehaviourSO values!");
        }
        public static T Pop()
        {
            return monoPool.Pop();
        }
        public static void Push(T instance)
        {
            Debug.Assert(instance != null, "local:push instance is null");
            monoPool.Push(instance);
        }
        public static void Clear()
        {
            monoPool.Clear();
        }
    }
}
