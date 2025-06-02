using UnityEngine;

public static partial class GeneralEditorUtility
{
    public static class ColorUtility
    {
        public static class Hierachy
        {
            public static Color Default { get; } = new Color(0.219f, 0.219f, 0.219f);
            public static Color Selected { get; } = new Color(0.27f, 0.27f, 0.27f);
            public static Color Hovering { get; } = new Color(0.27f, 0.27f, 0.27f);

            public static Color NewBlue { get; } = new Color(0, 0.54f, 0.9f);
        }
    }
}
