using UnityEngine;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEditor.Toolbars;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine.UIElements;
using System.Linq;

[Overlay(typeof(SceneView), "SceneLoadManager")]
public class ToolbarOverlay_SceneManager : UnityEditor.Overlays.ToolbarOverlay
{
    public const string ICON = GeneralEditorResource.GetRootResource + "_Icon/honghong.jpg";

    public ToolbarOverlay_SceneManager() : base(
        SceneDropdown.ID,
        FavoriteSceneDropdown.ID)
    {
        //todo : 
        //SceneViewCameraWindow.additionalSettingsGui += DoAdditionalCameraSettings;
    }
    static void DoAdditionalCameraSettings(SceneView sceneView)
    {
        GUILayout.Label("Additional Settings", EditorStyles.boldLabel);

        float easing = sceneView.cameraSettings.easingDuration;

        EditorGUI.BeginChangeCheck();

        easing = EditorGUILayout.Slider("Easing Duration", easing, 0.001f, 1f);

        if (EditorGUI.EndChangeCheck())
            sceneView.cameraSettings.easingDuration = easing;
    }
    public override VisualElement CreatePanelContent()
    {
        VisualElement result = new VisualElement();//CreateContent(Layout.VerticalToolbar);
        SceneView sceneView = SceneView.lastActiveSceneView;
        SceneViewCameraWindow sceneViewCamera = new SceneViewCameraWindow(sceneView);
        VisualElement v = sceneViewCamera.CreateGUI();
        result.Add(v);
        //result.style.width = new StyleLength(new Length(200, LengthUnit.Pixel));

        //Label lableTest = new Label("-Scene Manager-");
        //result.Add(lableTest);


        //Slider slider = new Slider("dawd", 0, 24);
        //slider.style.flexGrow = 0.1f;
        //result.Add(slider);

        //Image img = new Image();
        //img.image = EditorGUIUtility.FindTexture("ViewToolOrbit On@2x");
        //
        //result.Add(img);
        //slider.RegisterValueChangedCallback(
        //    (ChangeEvent<float> fce) =>
        //    {
        //        lableTest.text = fce.newValue.ToString();
        //    });
        return result;
    }

    private static void OpenScene(Scene currentScene, string newScenePath)
    {
        if (currentScene.isDirty)
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        }
        EditorSceneManager.OpenScene(newScenePath);
    }
    private static bool IsValidScene(string sceneName)
    {
        string[] prefixBlackList = { "~", "Basic", "Standard" };

        foreach (string bannedSceneName in prefixBlackList)
        {
            if (sceneName.StartsWith(bannedSceneName))
            {
                return false;
            }
        }
        return true;
    }
    private static string[] GetAllSceneGUIDs(string[] folders = null)
    {
        return folders != null ?
            AssetDatabase.FindAssets("t:scene") :
            AssetDatabase.FindAssets("t:scene", folders);
    }
    private static void ShowSceneMenu(string[] sceneGUIDs, bool skipValidation = false)
    {
        GenericMenu menu = new GenericMenu();
        Scene currentScene = SceneManager.GetActiveScene();//todo : get all additive scene

        int length = sceneGUIDs.Length;
        for (int i = 0; i < length; i++)
        {
            string path = sceneGUIDs[i].ConvertGUIDToPath();
            string sceneName = path.ConvertPathToName();
            if (!skipValidation && !IsValidScene(sceneName)) continue;

            menu.AddItem(new GUIContent(sceneName), string.Compare(currentScene.name, sceneName) == 0, () => OpenScene(currentScene, path));
        }
        menu.ShowAsContext();
    }

    [EditorToolbarElement(ID, typeof(SceneView))]
    public class SceneDropdown : EditorToolbarDropdown
    {
        public const string ID = "ToolbarOverlay/SceneDropdownToggle";
        public SceneDropdown()
        {
            text = "All Scene Man";
            tooltip = "I love you";

            icon = AssetDatabase.LoadAssetAtPath<Texture2D>(ICON);
            clicked += () => ShowSceneMenu(GetAllSceneGUIDs());
        }
    }

    [EditorToolbarElement(ID, typeof(SceneView))]
    public class FavoriteSceneDropdown : EditorToolbarDropdown
    {
        public const string ID = "ToolbarOverlay/FavoriteSceneDropdown";

        public FavoriteSceneDropdown()
        {
            text = "Fav scene dropdown";
            tooltip = "Toolt";
            icon = AssetDatabase.LoadAssetAtPath<Texture2D>(ICON);

            // todo : 
            //if (EditorBuildSettings.scenes[0] != null)
            //{
            //    Debug.Log(EditorBuildSettings.scenes[0].path);
            //}

            //can be null;
            const string soFullNamePath = GeneralEditorResource.GetRootResource + "EditorSceneRef.asset";
            EditorSceneRefSO editorSceneRef = AssetDatabase.LoadAssetAtPath<EditorSceneRefSO>(soFullNamePath);//.SceneRef;
            if (editorSceneRef != null)
            {
                foreach (SceneAsset item in editorSceneRef.GetSceneRef)
                {
                    Debug.Log(item.GetInstanceID().ConvertInstanceIDtoStr());
                    //clicked += () => {  };
                }
            }

            //clicked += () => { foreach (var aa in a) { Debug.Log(aa); } };//ShowSceneMenu(favoriteSceneGUIDs);
        }
        private void Test()
        {
            GenericMenu gm = new();
            //AssetDatabase.
            gm.AddItem(new GUIContent("sn"), true, null);
            gm.ShowAsContext();
        }
    }

    [EditorToolbarElement(ID, typeof(SceneView))]
    public class AssetImportToggle : EditorToolbarToggle
    {
        public const string ID = "ToolbarOverlay/AssetImportToggle";
        public AssetImportToggle()
        {
            text = "Toggle Asset Import";
            icon = AssetDatabase.LoadAssetAtPath<Texture2D>(ICON);

            this.RegisterValueChangedCallback(OnValueChanged);
        }
        public static void OnValueChanged(ChangeEvent<bool> changeEvent)
        {
            if (changeEvent.newValue)
            {
                AssetDatabase.StartAssetEditing();
            }
            else
            {
                AssetDatabase.StopAssetEditing();
            }
        }
    }
}
