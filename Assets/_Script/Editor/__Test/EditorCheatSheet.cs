using UnityEngine;
using UnityEditor;
public class EditorCheatSheet : EditorWindow
{
    //https://debuglog.tistory.com/category/%EC%95%84%EC%B9%B4%EC%9D%B4%EB%B9%99/Unity3D
    [MenuItem("Window/Editor Styles")]
    static void Init()
    {
        EditorCheatSheet window = GetWindow<EditorCheatSheet>();
        window.Show();
    }
    void OnGUI()
    {
        GUILayout.Label("Bold Label", EditorStyles.boldLabel);
        GUILayout.Label("Help Box", EditorStyles.helpBox);
        GUILayout.Label("Mini Bold Label", EditorStyles.miniBoldLabel);

        GUILayout.Button("Mini Button", EditorStyles.miniButton);
        GUILayout.Button("Mini Button Left", EditorStyles.miniButtonLeft);
        GUILayout.Button("Mini Button Mid", EditorStyles.miniButtonMid);
        GUILayout.Button("Mini Button Right", EditorStyles.miniButtonRight);

        GUILayout.Label("Mini Label", EditorStyles.miniLabel);
        GUILayout.TextField("Mini Text Field", EditorStyles.miniTextField);

        GUILayout.Label("Number Field", EditorStyles.numberField);
        GUILayout.Label("Object Field", EditorStyles.objectField);


        GUILayout.Label("Object Field Mini Thumb", EditorStyles.objectFieldMiniThumb);
        GUILayout.Space(20);
        GUILayout.Label("Object Field Thumb", EditorStyles.objectFieldThumb);

        GUILayout.Label("Popup", EditorStyles.popup);
        GUILayout.Button("Radio Button", EditorStyles.radioButton);

        GUILayout.TextArea("Text Area", EditorStyles.textArea);
        GUILayout.TextField("Text Field", EditorStyles.textField);

        GUILayout.Label("Toggle", EditorStyles.toggle);
        GUILayout.Label("Toggle Group", EditorStyles.toggleGroup);

        GUILayout.BeginHorizontal(EditorStyles.toolbar);
        GUILayout.Button("Toolbar Button", EditorStyles.toolbarButton);
        GUILayout.Button("Toolbar Drop Down", EditorStyles.toolbarDropDown);
        GUILayout.Button("Toolbar Popup", EditorStyles.toolbarPopup);
        GUILayout.TextField("Toolbar Text Field", EditorStyles.toolbarTextField);
        GUILayout.EndHorizontal();

        GUILayout.Label("White Bold Label", EditorStyles.whiteBoldLabel);
        GUILayout.Label("White Label", EditorStyles.whiteLabel);
        GUILayout.Label("White Large Label", EditorStyles.whiteLargeLabel);
        GUILayout.Label("White Mini Label", EditorStyles.whiteMiniLabel);

        GUILayout.Label("Word Wrapped Label", EditorStyles.wordWrappedLabel);
        GUILayout.Label("Word Wrapped Mini Label", EditorStyles.wordWrappedMiniLabel);
    }
}
