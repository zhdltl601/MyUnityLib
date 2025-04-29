using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
public static partial class CustomMenuItem
{
    private static class MissingScript
    {
        private const string ROOT = CUSTOM_TOOL_MENU + "/Missing Scripts/";

        private static void FindMissingScripts(Action<GameObject> OnFind)
        {
            GameObject[] arr = Object.FindObjectsByType<GameObject>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            int arrLength = arr.Length;
            for (int i = 0; i < arrLength; i++)
            {
                GameObject currentGameObject = arr[i];
                MonoBehaviour[] monoInCurrentGameObject = currentGameObject.GetComponentsInChildren<MonoBehaviour>(true);

                int monoBehaviourLength = monoInCurrentGameObject.Length;
                for (int j = 0; j < monoBehaviourLength; j++)
                {
                    MonoBehaviour currentMono = monoInCurrentGameObject[j];
                    if (currentMono == null)
                    {
                        OnFind.Invoke(currentGameObject);
                        break;
                    }
                }
            }
        }

        private const string FIND = ROOT + "Find";
        [MenuItem(FIND)]
        public static void FindMissingScriptsLog()
        {
            FindMissingScripts(
                (gameObject) =>
                {
                    Debug.Log($"Missing Script : {gameObject.name}", gameObject);
                });
        }

        private const string REMOVE = ROOT + "Remove";
        [MenuItem(REMOVE)]
        public static void FindMissingScriptsRemove()
        {
            FindMissingScripts(
                (gameObject) =>
                {
                    Debug.Log($"Removed Missing Script : {gameObject.name}", gameObject);
                    GameObjectUtility.RemoveMonoBehavioursWithMissingScript(gameObject);
                });
        }
    }
}