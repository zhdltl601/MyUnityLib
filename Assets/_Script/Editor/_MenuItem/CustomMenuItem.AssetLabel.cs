using UnityEngine;
using UnityEditor;
public static partial class CustomMenuItem
{
    private static class AssetLabel
    {
        private const string ROOT = CUSTOM_TOOL_MENU + "/AssetLabel/";

        private const string ADD = ROOT + "AddAssetLabel";

        [MenuItem(ADD)]
        public static void AddAssetLabel()
        {
            Object[] selectedObjects = Selection.objects;
            if (selectedObjects == null) return;

            foreach (Object unityObject in selectedObjects)
            {
                string[] currentLabels = AssetDatabase.GetLabels(unityObject); 
                const string labelName = "TestLabel";
                if (ArrayUtility.Contains(currentLabels, labelName)) continue;

                ArrayUtility.Add(ref currentLabels, labelName);
                AssetDatabase.SetLabels(unityObject, currentLabels);
            }
        }
    }
}
