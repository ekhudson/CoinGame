using System.Collections;

using UnityEngine;
using UnityEditor;

public static class GrendelEditorStyles
{
    public static GUIStyle toolbarSearchField
    {
        get
        {
            return (GUIStyle)EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector).GetStyle("toolbarSearchField");
        }
    }

    public static GUIStyle toolbarSearchFieldPopup
    {
        get
        {
            return (GUIStyle)EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector).GetStyle("toolbarSearchFieldPopup");
        }
    }

    public static GUIStyle toolbarSearchFieldCancelButton
    {
        get
        {
            return (GUIStyle)EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector).GetStyle("toolbarSearchFieldCancelButton");
        }
    }

    public static GUIStyle toolbarSearchFieldCancelButtonEmpty
    {
        get
        {
            return (GUIStyle)EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector).GetStyle("toolbarSearchFieldCancelButtonEmpty");
        }
    }

    public static GUIStyle colorPickerBox
    {
        get
        {
            return (GUIStyle)EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector).GetStyle("colorPickerBox");
        }
    }

    public static GUIStyle inspectorBig
    {
        get
        {
            return (GUIStyle)EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector).GetStyle("inspectorBig");
        }
    }

    public static GUIStyle inspectorTitlebar
    {
        get
        {
            return (GUIStyle)EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector).GetStyle("inspectorTitlebar");
        }
    }

    public static GUIStyle inspectorTitlebarText
    {
        get
        {
            return (GUIStyle)EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector).GetStyle("inspectorTitlebarText");
        }
    }

    public static GUIStyle foldoutSelected
    {
        get
        {
            return (GUIStyle)EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector).GetStyle("foldoutSelected");
        }
    }

    public static GUIStyle notificationText
    {
        get
        {
            return (GUIStyle)EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector).GetStyle("notificationText");
        }
    }

    public static GUIStyle notificationTextBackground
    {
        get
        {
            return (GUIStyle)EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector).GetStyle("notificationTextBackground");
        }
    }

    public static GUIStyle helpBox
    {
        get
        {
            return (GUIStyle)EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector).GetStyle("helpBox");
        }
    }

    public static GUIStyle tagTextField
    {
        get
        {
            return (GUIStyle)EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector).GetStyle("tagTextField");
        }
    }

    public static GUIStyle tagTextFieldEmpty
    {
        get
        {
            return (GUIStyle)EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector).GetStyle("tagTextFieldEmpty");
        }
    }

    public static GUIStyle tagTextFieldButton
    {
        get
        {
            return (GUIStyle)EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector).GetStyle("tagTextFieldButton");
        }
    }

    public static GUIStyle searchField
    {
        get
        {
            return (GUIStyle)EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector).GetStyle("searchField");
        }
    }

    public static GUIStyle searchFieldCancelButton
    {
        get
        {
            return (GUIStyle)EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector).GetStyle("searchFieldCancelButton");
        }
    }

    public static GUIStyle searchFieldCancelButtonEmpty
    {
        get
        {
            return (GUIStyle)EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector).GetStyle("searchFieldCancelButtonEmpty");
        }
    }

    public static GUIStyle selectionRect
    {
        get
        {
            return (GUIStyle)EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector).GetStyle("selectionRect");
        }
    }

    public static GUIStyle minMaxHorizontalSliderThumb
    {
        get
        {
            return (GUIStyle)EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector).GetStyle("minMaxHorizontalSliderThumb");
        }
    }

    public static GUIStyle progressBarBack
    {
        get
        {
            return (GUIStyle)EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector).GetStyle("progressBarBack");
        }
    }

    public static GUIStyle progressBarBar
    {
        get
        {
            return (GUIStyle)EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector).GetStyle("progressBarBar");
        }
    }

    public static GUIStyle progressBarText
    {
        get
        {
            return (GUIStyle)EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector).GetStyle("progressBarText");
        }
    }
}
