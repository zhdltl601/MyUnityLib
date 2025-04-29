using UnityEditor;
using UnityEditor.ShortcutManagement;
using UnityEngine;

public class EditorShortcut
{
    [Shortcut("Window/Close", KeyCode.W, ShortcutModifiers.Control)]
    private static void CloseTab()
    {
        if (EditorWindow.focusedWindow == null) return;
        EditorWindow.focusedWindow.Close();
    }

    [Shortcut("Window/Ping", KeyCode.Q, ShortcutModifiers.Shift | ShortcutModifiers.Control)]
    private static void PingObject()
    {
        EditorGUIUtility.PingObject(Selection.activeObject);
    }
}