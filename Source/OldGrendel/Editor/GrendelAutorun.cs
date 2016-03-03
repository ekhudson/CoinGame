using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class GrendelAutorun
{
    static GrendelAutorun()
    {
        SceneView.onSceneGUIDelegate += EditorInput.Update;
        EditorApplication.update += GameManagerEditor.Update;
        SceneView.onSceneGUIDelegate += GrendelManager.Update;
    }
}
