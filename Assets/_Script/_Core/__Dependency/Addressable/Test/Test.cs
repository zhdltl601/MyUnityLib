using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Test : MonoBehaviour
{
    [SerializeField] private AssetReferenceGameObject refGameObject;
    private AsyncOperationHandle<GameObject> handle;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            TestLoad();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            TestUnload();
        }

    }
    public void TestLoad()
    {
        refGameObject.LoadAssetAsync()
            .Completed +=
            (AsyncOperationHandle<GameObject> handle) =>
            {
                this.handle = handle;
                Instantiate(handle.Result);
            };
    }
    public void TestUnload()
    {
        Addressables.Release(handle);
    }
}
