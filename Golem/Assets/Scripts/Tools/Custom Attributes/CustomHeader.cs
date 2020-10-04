using UnityEngine;

public class CustomHeader : PropertyAttribute
{
    public string title;
    public int size;
    public Color color;

    public GUIContent content;
    public GUIStyle style;

    public float topPadding = 10f;
    public float bottomPadding = 5f;

    public CustomHeader(string title, int size = 13, FontStyle fontStyle = FontStyle.Bold, float r = 1f, float g = 0f, float b = 1f)
    {
        this.title = title;
        this.size = size;
        this.color = new Color(r, g, b);

        content = new GUIContent(title);
        style = new GUIStyle();

        style.fontSize = size;
        style.normal.textColor = this.color;
        style.fontStyle = fontStyle;
    }
}