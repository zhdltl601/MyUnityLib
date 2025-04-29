using UnityEngine;

public class PropertyTest : PropertyAttribute
{
    public int thickness = 1;
    public float padding = 0;
    public PropertyTest(int thickness, float padding)
    {
        this.thickness = thickness;
        this.padding = padding;
    }
}
