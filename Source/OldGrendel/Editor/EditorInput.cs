using System.Collections;

using UnityEngine;
using UnityEditor;

public class EditorInput : Editor 
{
    public static void Update(SceneView sv)
    {
        Event e = Event.current;

        if (e == null)
        {
            return;
        }                
        
        if (e.type == EventType.keyDown)
        {
            switch(e.keyCode)
            {
                case KeyCode.L:
                
                    GrendelEditorPreferences.DrawEditorObjectLabels = !GrendelEditorPreferences.DrawEditorObjectLabels;
                
                break;
            
                case KeyCode.End:

                    MoveObjectToGround.Move(Selection.activeGameObject);
                
                break;
            }            
        }
    }    
}
