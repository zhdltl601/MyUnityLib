using System;
using UnityEditor;
using UnityEngine;
public static partial class CustomMenuItem
{
    public const string CUSTOM_TOOL_MENU = "CustomTool";
    [MenuItem(CUSTOM_TOOL_MENU + "/TestFucker")]
    private static void Function()
    {
        Debug.Log(GeneralEditorResource.GetRootResource);

    }
}

