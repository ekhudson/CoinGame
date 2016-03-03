using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : GrendelEditor<GameManager>
{
	public static void Update()
	{
        GameManager target = (GameManager)GameObject.FindObjectOfType(typeof(GameManager));

        if (target == null)
        {
            return;
        }

        if (!EditorApplication.isPlaying && EditorApplication.isPlayingOrWillChangePlaymode)
        {
            if (target.SceneToLoadOnGameLaunch != null && EditorApplication.currentScene != target.SceneToLoadOnGameLaunch.name && GrendelEditorPreferences.AskToLoadStartupScene)
            {
                if ( EditorUtility.DisplayDialog("Load Startup Scene", string.Format("You are not in the default scene. Do you want to play from default scene \"{0}\"", target.SceneToLoadOnGameLaunch.name), "No", "Yes") )
                {
                    target.LoadDefaultScene = false;
                }
                else
                {
                    target.LoadDefaultScene = true;
                }
            }
            else
            {
                target.LoadDefaultScene = false;
            }
        }
        else if (!EditorApplication.isPlaying)
        {
            target.LoadDefaultScene = false;
        }

        EditorUtility.SetDirty(target);
	}

}

