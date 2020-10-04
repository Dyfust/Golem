using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(CustomHeader))]
public class CustomHeaderDrawer : DecoratorDrawer
{
    CustomHeader customHeader
    {
        get { return (CustomHeader)attribute; }
    }

    public override float GetHeight()
    {
        return base.GetHeight() + customHeader.topPadding;
    }

    public override void OnGUI(Rect position)
    {
        position.y = position.y + customHeader.topPadding + (customHeader.size / 2f - customHeader.bottomPadding);
        EditorGUI.LabelField(position, customHeader.content, customHeader.style);
    }
}