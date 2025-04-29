using UnityEngine;

namespace Custom.Pool
{
    internal class GameObjectPool : UnityObjectPool<GameObject>
    {
        //protected override string PoolParentName => $"UnityObjectPool_{nameof(prefab)}";
        public GameObjectPool(GameObject prefab, int initialPoolCapacity = 10, int maxCapacity = 1000, int preCreate = 10) : base(prefab, initialPoolCapacity, maxCapacity, preCreate)
        {
            UnityObjectPool.SceneChangePoolDestroyEvent += base.Clear;
        }
        ~GameObjectPool()
        {
            base.Clear();
            UnityObjectPool.SceneChangePoolDestroyEvent -= base.Clear;
        }
        protected override GameObject Create()
        {
            GameObject result = Object.Instantiate(prefab, Vector3.zero, Quaternion.identity);
            result.gameObject.hideFlags = HideFlags.HideInHierarchy;
            result.SetActive(false);
            return result;
        }
        protected override void Destroy(GameObject instance)
        {
            Object.Destroy(instance);
        }
        public override GameObject Pop()
        {
            GameObject result =  base.Pop();
            result.gameObject.hideFlags = HideFlags.None;
            result.SetActive(true);
            return result;
        }
        public override void Push(GameObject instance)
        {
            instance.gameObject.hideFlags = HideFlags.HideInHierarchy;
            instance.SetActive(false);
            base.Push(instance);
        }
        public override void Clear()
        {
            foreach (GameObject item in poolList)
            {
                Object.Destroy(item);
            }
            base.Clear();
        }

    }
}
