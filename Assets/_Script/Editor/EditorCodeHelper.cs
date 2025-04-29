using System;
using UnityEditor;
using UnityEngine;
using UnityEditor.ProjectWindowCallback;
using System.IO;
public static class EditorCodeHelper
{
    public enum ECreationMode
    {
        ChildClass,
        CustomInspector
    }
    public class EndNameEditHandler : EndNameEditAction
    {
        private ECreationMode creationMode;
        private Type targetType;

        public void Configure(ECreationMode mode, Type referenceType)
        {
            creationMode = mode;
            targetType = referenceType;
        }
        private string GetTempalteName(ECreationMode mode) => mode switch
        {
            ECreationMode.ChildClass => "CodeTemplate_ChildClass",
            ECreationMode.CustomInspector => "CodeTemplate_CustomInspector",
            _ => throw new ArgumentOutOfRangeException($"Unknown mode {mode}")
        };
        private string GetTemplateText(ECreationMode creationMode)
        {
            string[] templateGUIDs = AssetDatabase.FindAssets($"{GetTempalteName(creationMode)} t:TextAsset");

            if (templateGUIDs.Length != 1)
            {
                throw new Exception("found mutiple template or none");
            }

            return File.ReadAllText(AssetDatabase.GUIDToAssetPath(templateGUIDs[0]));
        }

        public override void Action(int instanceId, string pathName, string resourceFile)
        {
            string newFileContent = GetTemplateText(creationMode);
            string newClassName = Path.GetFileNameWithoutExtension(pathName);

            newFileContent = newFileContent.Replace("#CLASSNAME#", newClassName);
            newFileContent = newFileContent.Replace("#PARENTCLASSNAME#", targetType.Name);

            if (creationMode == ECreationMode.CustomInspector)
            {
                string[] pathElements = pathName.Split(Path.DirectorySeparatorChar);

                bool isInEditorFolder = false;
                foreach (var element in pathElements)
                {
                    if (element.ToLower() == "editor")
                    {
                        isInEditorFolder = true;
                        break;
                    }
                }

                // not in editor folder
                if (!isInEditorFolder)
                {
                    string basePath = Path.GetDirectoryName(pathName);
                    string fileName = Path.GetFileName(pathName);

                    string newPath = Path.Combine(basePath, "Editor");
                    Directory.CreateDirectory(newPath);

                    pathName = Path.Combine(newPath, fileName);
                }
            }

            File.WriteAllText(pathName, newFileContent);

            AssetDatabase.ImportAsset(pathName);
            MonoScript newScript = AssetDatabase.LoadAssetAtPath<MonoScript>(pathName);
            ProjectWindowUtil.ShowCreatedAsset(newScript);
        }
    }

    private static bool IsSelectedOnlyOneMonoScript => Selection.activeObject is MonoScript && Selection.count == 1;

    [MenuItem("Assets/Code Helpers/Create Child Class", true)] private static bool AddChildClassValidation() => IsSelectedOnlyOneMonoScript;
    [MenuItem("Assets/Code Helpers/Create Child Class")]
    private static void AddChildClass()
    {
        PerformCreation(ECreationMode.ChildClass, "Child");
    }

    [MenuItem("Assets/Code Helpers/Create Custom Inspector", true)] private static bool AddCustomInspectorValidation() => IsSelectedOnlyOneMonoScript;
    [MenuItem("Assets/Code Helpers/Create Custom Inspector")]
    private static void AddCustomInspector()
    {
        PerformCreation(ECreationMode.CustomInspector, "Editor");
    }

    private static void PerformCreation(ECreationMode mode, string suffix)
    {
        MonoScript scriptAsset = Selection.activeObject as MonoScript;
        Type classType = scriptAsset.GetClass();
        string fileName = $"{classType.Name}{suffix}.cs";

        EndNameEditHandler endNameEditHandler = ScriptableObject.CreateInstance<EndNameEditHandler>();
        endNameEditHandler.Configure(mode, classType);

        ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, endNameEditHandler, fileName, null, null);
    }
}
