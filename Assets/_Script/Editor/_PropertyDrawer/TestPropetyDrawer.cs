using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(PropertyTest))]
public class TestPropetyDrawer : DecoratorDrawer
{
    public override float GetHeight()
    {
        PropertyTest propertyTest = attribute as PropertyTest;
        return Mathf.Max(propertyTest.padding, propertyTest.thickness);
    }
    public override void OnGUI(Rect position)
    {
        PropertyTest attr = attribute as PropertyTest;
        position.height = attr.thickness;
        position.y += attr.padding * .5f;

        EditorGUI.DrawRect(position, Color.red);
    }
}
