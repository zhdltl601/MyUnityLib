using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

[InitializeOnLoad]
public static class HierachyDisplayer
{
    static HierachyDisplayer()
    {
        EditorApplication.hierarchyWindowItemOnGUI += HandleOnHierarchyWindowItemOnGUI;

        SceneView.duringSceneGui += OrbitVisual;
    }
    private static void HandleOnHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
    {
        GameObjectToggle(instanceID, selectionRect);
        CustomHierarchy(instanceID, selectionRect);
    }
    private static void OrbitVisual(SceneView obj)
    {
        if (Event.current.alt)
        {
            Vector3 pivot = obj.pivot;
            float handleSize = HandleUtility.GetHandleSize(pivot);
            Vector3 upPoint = Vector3.up * 1.25f;

            Handles.DrawWireArc(pivot, Vector3.up, Vector3.forward, 360, handleSize);
            Handles.DrawLine(pivot, pivot + upPoint);
            Handles.DrawLine(pivot + Vector3.right * handleSize, pivot + upPoint);
        }
    }
    private static void GameObjectToggle(int instanceID, Rect selectionRect)
    {
        GameObject gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
        if (gameObject != null)
        {
            Rect toggleRect = new Rect(selectionRect);
            toggleRect.x -= 27f;
            toggleRect.width = 13f;
            bool active = EditorGUI.Toggle(toggleRect, gameObject.activeSelf);
            if (active != gameObject.activeSelf)
            {
                Undo.RecordObject(gameObject, "Toggle Active State");
                gameObject.SetActive(active);
            }
        }
    }
    private static void CustomHierarchy(int instanceID, Rect selectionRect)
    {
        GameObject gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
        if (gameObject == null)
            return;

        if (PrefabUtility.GetCorrespondingObjectFromOriginalSource(gameObject) != null)
            return;

        Component[] components = gameObject.GetComponents<Component>();

        Component componentBackground = null;
        Component componentMono = null;

        int componentLength = components.Length;
        for (int i = 1; i < componentLength; i++)
        {
            Component item = components[i];

            bool monoInitFlag = componentMono == null;
            bool backgroundInitFlag = componentBackground == null;

            MonoBehaviour itemAsMono = item as MonoBehaviour;
            bool isItemMono = itemAsMono != null;

            if (monoInitFlag && isItemMono)
            {
                string namespaceName = itemAsMono.GetType().Namespace;

                //ban list
                bool isBannedNamespace = namespaceName != null
                    && (namespaceName.StartsWith(nameof(UnityEngine))
                    || namespaceName.StartsWith(nameof(UnityEditor))
                    || namespaceName.StartsWith(nameof(TMPro)));

                if (!isBannedNamespace)
                {
                    componentMono = itemAsMono;
                }
            }
            if (backgroundInitFlag && !isItemMono)
            {
                componentBackground = item;
            }

            bool breakFlag = !backgroundInitFlag && !monoInitFlag;
            if (breakFlag) break;
        }
        if (componentBackground == null)
        {
            componentBackground = components[0];
        }

        Type type = componentBackground.GetType();
        Object targetContentObject = componentBackground;

        GUIContent content = EditorGUIUtility.ObjectContent(targetContentObject, type);
        content.tooltip = content.text;
        content.text = "";

        if (content == null)
            return;

        bool isSelected = Selection.instanceIDs.Contains(instanceID);
        bool isHovering = selectionRect.Contains(Event.current.mousePosition);

        Rect backgroundRect = selectionRect;
        backgroundRect.width = 17f;
        Color backgroundColor = GetColor(isSelected, isHovering);

        //clear
        EditorGUI.DrawRect(backgroundRect, backgroundColor);

        //background draw
        EditorGUI.LabelField(selectionRect, content);

        //small icon draw
        if (componentMono != null)
        {
            content.image = EditorGUIUtility.GetIconForObject(componentMono);
            if (content.image == null)
            {
                content.image = GeneralEditorUtility.Icons.ICON_MONO;
            }

            Rect smallIconRect = selectionRect;
            smallIconRect.width = 11.25f;
            smallIconRect.height = 11.25f;
            smallIconRect.position += new Vector2(9.25f, 5);
            EditorGUI.LabelField(smallIconRect, content);
        }
    }
    private static Color GetColor(bool isSelected, bool isHovering)
    {
        if (isSelected)
        {
            return GeneralEditorUtility.ColorUtility.Hierachy.Selected;
        }
        if (isHovering)
        {
            return GeneralEditorUtility.ColorUtility.Hierachy.Hovering;
        }
        return GeneralEditorUtility.ColorUtility.Hierachy.Default;
    }
}
