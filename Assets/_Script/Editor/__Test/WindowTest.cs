using UnityEditor;
using UnityEngine;

public class WindowTest : EditorWindow
{
    private int intVal;
    private float floatVal;
    private float minVal;
    private float maxVal;

    [MenuItem("CustomTool/Fuck")]
    private static void SetUp()
    {
        WindowTest window = GetWindow<WindowTest>();
        window.titleContent = new GUIContent("fuck", "tooltip");
        window.minSize = new Vector2(10, 10);
        window.maxSize = new Vector2(640, 480);
    }
}
