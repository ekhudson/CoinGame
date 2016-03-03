using System.Collections;

using UnityEngine;
using UnityEditor;

public static class GrendelEditorPreferences
{
    
    public static bool DrawEditorObjectLabels
    {
        get
        {
            if (!EditorPrefs.HasKey("DrawEditorObjectLabels"))
            {
                EditorPrefs.SetBool("DrawEditorObjectLabels", true);
            }
            
            return EditorPrefs.GetBool("DrawEditorObjectLabels");            
        }
        
        set
        {
            EditorPrefs.SetBool("DrawEditorObjectLabels", value);
        }
    }

    public static bool AskToLoadStartupScene
    {
        get
        {
            if (!EditorPrefs.HasKey("AskToLoadStartupScene"))
            {
                EditorPrefs.SetBool("AskToLoadStartupScene", true);
            }
            
            return EditorPrefs.GetBool("AskToLoadStartupScene");
        }

        set
        {
            EditorPrefs.SetBool("AskToLoadStartupScene", value);
        }
    }
}