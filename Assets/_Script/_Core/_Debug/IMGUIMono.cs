using System.Collections.Generic;
using UnityEngine;
public enum DebugKey
{
    none,
    temp1,
    temp2,
    temp3
}
[DefaultExecutionOrder(-200)]
public class IMGUIMono : MonoSingleton<IMGUIMono>
{
    protected override MonoSingletonFlags SingletonFlag => MonoSingletonFlags.DontDestroyOnLoad;
    public static DebugKey debugKey = DebugKey.none;

    private static bool EnableGUI { get; set; } = true;

    private static readonly GUIStyle gUIStyle = new GUIStyle();
    private static readonly GUIStyle style = gUIStyle;
    private static readonly List<DebugWorldInfo> worldInfoList = new List<DebugWorldInfo>(32);

    [SerializeField] private float screenMultiplier = 0.03f;

    protected override void Awake()
    {
        base.Awake();
        style.alignment = TextAnchor.UpperLeft;
    }
    private void Update()
    {
        worldInfoList.Clear();
    }
    public static void DebugTextWorld<T>(T target, Vector3 worldPosition, float sizeRatio = 1, DebugKey key = DebugKey.none)
    {
        if (debugKey != key) return;

        DebugWorldInfo debugWorldInfo = new DebugWorldInfo(worldPosition, target.ToString(), sizeRatio);
        worldInfoList.Add(debugWorldInfo);
    }
    private void OnGUI()
    {
        if (!EnableGUI) return;

        Camera camera = Camera.main;
        int screenHeight = Screen.height;
        int fontSize = (int)(screenHeight * screenMultiplier);

        foreach (DebugWorldInfo item in worldInfoList)
        {
            style.fontSize = (int)(fontSize * item.sizeRatio);
            Vector3 screenPosition = camera.WorldToScreenPoint(item.worldPosition, Camera.MonoOrStereoscopicEye.Mono);
            screenPosition.y = screenHeight - screenPosition.y;

            Vector2 size = new Vector3(2000, 2000);
            Rect position = new Rect(screenPosition, size);

            GUI.Label(position, item.message, style);
        }
    }
}
