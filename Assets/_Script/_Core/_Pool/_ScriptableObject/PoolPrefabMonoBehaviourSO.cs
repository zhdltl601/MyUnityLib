using UnityEngine;

namespace Custom.Pool
{
    [CreateAssetMenu(fileName = "PoolPrefabMonoBehaviourSO", menuName = "Scriptable Objects/Pool/PrefabMonoBehaviourSO")]
    public class PoolPrefabMonoBehaviourSO : PoolPrefabUnityBaseSO
    {
        [SerializeField] private MonoBehaviour mono;
        public MonoBehaviour GetMono => mono;
        protected override void OnValidate()
        {
            base.OnValidate();
            if (mono != null)
            {
                if (mono.gameObject != prefab)
                {
                    mono = null;
                    Debug.LogError($"Mono and prefab GameObjects are different. {name}");
                }
            }
        }
    }
}
