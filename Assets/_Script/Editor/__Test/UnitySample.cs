using UnityEngine;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEditor.Toolbars;
using UnityEngine.UIElements;

[EditorToolbarElement(ID, typeof(SceneView))]
class DropdownExample : EditorToolbarDropdown
{
    public const string ID = "ExampleToolbar/Dropdown";

    static string dropChoice = null;

    public DropdownExample()
    {
        text = "Axis";
        clicked += ShowDropdown;
    }

    void ShowDropdown()
    {
        var menu = new GenericMenu();
        menu.AddItem(new GUIContent("X"), dropChoice == "X", () => { text = "X"; dropChoice = "X"; });
        menu.AddItem(new GUIContent("Y"), dropChoice == "Y", () => { text = "Y"; dropChoice = "Y"; });
        menu.AddItem(new GUIContent("Z"), dropChoice == "Z", () => { text = "Z"; dropChoice = "Z"; });
        menu.ShowAsContext();
    }
}
[EditorToolbarElement(ID, typeof(SceneView))]
class ToggleExample : EditorToolbarToggle
{
    public const string ID = "ExampleToolbar/Toggle";
    public ToggleExample()
    {
        text = "Toggle OFF"; 
        this.RegisterValueChangedCallback(Test); 
    }

    private void Test(ChangeEvent<bool> evt)
    {
        if (evt.newValue)
        {
            Debug.Log("ON");
            text = "Toggle ON";
        }
        else
        {
            Debug.Log("OFF");
            text = "Toggle OFF";
        }
    }
}

[EditorToolbarElement(ID, typeof(SceneView))]
class DropdownToggleExample : EditorToolbarDropdownToggle, IAccessContainerWindow
{
    public const string ID = "ExampleToolbar/DropdownToggle";


    public EditorWindow containerWindow { get; set; }
    static int colorIndex = 0;
    static readonly Color[] colors = new Color[] { Color.red, Color.green, Color.cyan };
    public DropdownToggleExample()
    {
        text = "Color Bar";
        tooltip = "Display a color rectangle in the top left of the Scene view. Toggle on or off, and open the dropdown" +
                  "to change the color.";

        // When the dropdown is opened, ShowColorMenu is invoked and we can create a popup menu.
        dropdownClicked += ShowColorMenu;

        // Subscribe to the Scene view OnGUI callback so that we can draw our color swatch.

        SceneView.duringSceneGui += DrawColorSwatch;
    }

    void DrawColorSwatch(SceneView view)
    {
        // Test that this callback is for the Scene View that we're interested in, and also check if the toggle is on
        // or off (value).

        if (!value)
        {
            return;
        }

        Handles.BeginGUI();
        GUI.color = colors[colorIndex];
        GUI.DrawTexture(new Rect(8, 8, 120, 24), Texture2D.whiteTexture);
        GUI.color = Color.white;
        Handles.EndGUI();
    }
    void ShowColorMenu()
    {
        var menu = new GenericMenu();
        menu.AddItem(new GUIContent("Red"), colorIndex == 0, () => colorIndex = 0);
        menu.AddItem(new GUIContent("Green"), colorIndex == 1, () => colorIndex = 1);
        menu.AddItem(new GUIContent("Blue"), colorIndex == 2, () => colorIndex = 2);
        menu.ShowAsContext();
    }
}

[EditorToolbarElement(ID, typeof(SceneView))]
class CreateCube : EditorToolbarButton
{
    public const string ID = "ExampleToolbar/Button";

    public CreateCube()
    {
        text = "Create Cube";
        icon = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/CreateCubeIcon.png");
        tooltip = "Instantiate a cube in the scene.";
        clicked += OnClick;
    }

    void OnClick()
    {
        var newObj = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;

        Undo.RegisterCreatedObjectUndo(newObj.gameObject, "Create Cube");
    }

}

[Overlay(typeof(SceneView), "ElementToolbars Example")]
[Icon("Assets/unity.png")]
public class EditorToolbarExample : ToolbarOverlay
{
    EditorToolbarExample() : base(
        CreateCube.ID,
        ToggleExample.ID,
        DropdownExample.ID,
        DropdownToggleExample.ID
        )
    { }
}