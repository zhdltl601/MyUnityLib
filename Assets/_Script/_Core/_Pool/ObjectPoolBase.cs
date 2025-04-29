using System;
using System.Collections.Generic;

namespace Custom.Pool
{
    internal static class ObjectPoolBase
    {
        internal static bool collisionCheck = false;
#if UNITY_EDITOR
        static ObjectPoolBase()
        {
            collisionCheck = true;
        }
#endif  

    }

    public abstract class ObjectPoolBase<T>
        where T : class
    {
        public event Action ClearEvent;

        public IReadOnlyList<T> GetList => poolList;
        protected readonly List<T> poolList;
        private readonly int maxCapacity;

        public ObjectPoolBase(int initialPoolCapacity = 10, int maxCapacity = 1000)
        {
            poolList = new List<T>(initialPoolCapacity);
            this.maxCapacity = maxCapacity;
        }
        public virtual T Pop()
        {
            T result;
            int lastIndex = poolList.Count - 1;
            if (lastIndex == -1)
                result = Create();
            else
            {
                result = poolList[lastIndex];
                poolList.RemoveAt(lastIndex);
            }
            return result;
        }
        public virtual void Push(T instance)
        {
            bool collision = ObjectPoolBase.collisionCheck;
            if (collision)
            {
                foreach (T item in poolList)
                {
                    if (item == instance)
                        throw new Exception($"Collision Detected. prefab : {instance}, {nameof(T)}_POOL");
                }
            }

            if (poolList.Count < maxCapacity)
            {
                poolList.Add(instance);
            }
            else
                Destroy(instance);
        }
        protected abstract T Create();
        protected abstract void Destroy(T instance);
        
        public virtual void Clear()
        {
            ClearEvent?.Invoke();
            poolList.Clear();
        }
    }
}
