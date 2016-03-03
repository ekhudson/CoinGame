using UnityEngine;
using UnityEditor;

using System.Collections;

[CustomPropertyDrawer(typeof(GrendelSeparator))]
public class GrendelSeparatorDrawer : PropertyDrawer
{
    private const float kIndentPercent = 0.15f;

    public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
    {
        float indent = (pos.width * kIndentPercent);

        GUI.Box(new Rect(pos.x + (indent * 0.5f), pos.y + (pos.height * 0.5f), pos.width - indent, 1), string.Empty, GUI.skin.textArea);
    }
}
