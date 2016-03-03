using UnityEngine;
using UnityEditor;

using System.Collections;
using System.Collections.Generic;

public class GrendelManager : MonoBehaviour
{
    private static GrendelProjectData mProjectData = null;
    private static bool mProjectDataWarningIgnored = false;
    private const float kToolbarButtonSpacer = 8f;
    private const float kGrendelToolbarHeight = 32f;
    private const float kGrendelToolbarWidth = 1024f;
    private static GUIStyle kGrendelToolbarStyle = null;
    private static SceneView mSceneView;
    private static Texture2D mToolbarTexture = null;
    private static Texture2D mGrendelIcon = null;

    //Grendel Prefs, should be moved to their own place
    private static bool mDrawPaths = true;

    public static bool DrawPaths
    {
        get
        {
            return mDrawPaths;
        }
    }

    public static GrendelProjectData ProjectData
    {
        get
        {
            return mProjectData;
        }
    }

    public static void Update(SceneView sv)
    {
        mSceneView = sv;

        if (kGrendelToolbarStyle == null)
        {
            mToolbarTexture = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Third Party/Grendel/Textures/toolbarTexture.png", typeof(Texture));
            mGrendelIcon = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Third Party/Grendel/Textures/Grendel_Icon_Large_White.png", typeof(Texture));

            kGrendelToolbarStyle = new GUIStyle(EditorStyles.toolbar);

            kGrendelToolbarStyle.normal.background = mToolbarTexture;
        }

        GUI.Window(1001, new Rect(0, SceneView.kToolbarHeight, sv.position.width, kGrendelToolbarHeight), GrendelToolbarWindow, string.Empty, GUIStyle.none);
    }

    public static void GrendelToolbarWindow(int windowID)
    {
        Event evt = Event.current;
        Rect toolbarRect = new Rect(0, 0, mSceneView.position.width, kGrendelToolbarHeight);

        GUILayout.BeginArea(toolbarRect, string.Empty, kGrendelToolbarStyle);

        EditorGUILayout.BeginHorizontal();

        if(GUILayout.Button(mGrendelIcon, EditorStyles.toolbarButton, GUILayout.Width(kGrendelToolbarHeight)))
        {
            Rect buttonRect = GUILayoutUtility.GetLastRect();
            buttonRect.y += kGrendelToolbarHeight * 0.5f;
            EditorUtility.DisplayPopupMenu(buttonRect, "Grendel", null);
        }

        GUILayout.Space(kToolbarButtonSpacer);

        if (mProjectData == null && !mProjectDataWarningIgnored)
        {
            DrawMissingDataWarning();
        }
        else
        {
            if(GUILayout.Button("Project Settings", EditorStyles.toolbarButton))
            {
                if (Selection.activeObject == mProjectData)
                {
                    EditorUtility.FocusProjectWindow();
                    return;
                }

                EditorGUIUtility.PingObject(mProjectData);
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = mProjectData;
            }

            GUILayout.Space(kToolbarButtonSpacer);
        }

            mDrawPaths = GUILayout.Toggle(mDrawPaths, "Draw Paths", EditorStyles.toolbarButton);

            GUILayout.Space(kToolbarButtonSpacer);

        GUILayout.FlexibleSpace();

        EditorGUILayout.EndHorizontal();

        GUILayout.EndArea();

        if (toolbarRect.Contains(evt.mousePosition) && evt.type == EventType.MouseDown)
        {
            evt.Use();
        }
    }

    //TODO: Replace with a general purpose toolbar notification system
    public static void DrawMissingDataWarning()
    {
        GUI.color = Color.yellow;
        GUILayout.Label("Project Data File Missing! Grendel Framework Non-Functional.", EditorStyles.toolbarTextField);
        GUI.color = Color.white;
        
        GUILayout.Space(kToolbarButtonSpacer);
        
        if (GUILayout.Button("Click here to create a Data File for this project", EditorStyles.toolbarButton))
        {
            mProjectData = GrendelProjectDataEditor.CreateProjectDataAsset();
        }
        
        GUILayout.FlexibleSpace();
        
        LookForProjectData();
    }

    public static void LookForProjectData()
    {
        GrendelProjectData data = (GrendelProjectData)FindAssetOfType(typeof(GrendelProjectData));

        if (data == null)
        {
            return;
        }

        mProjectData = data;
    }

    public static Object FindAssetOfType(System.Type type)
    {
        string[] paths = System.IO.Directory.GetFiles(System.IO.Path.Combine(Application.dataPath, "Resources/Grendel/") );

        foreach (string path in paths)
        {
            Object obj = AssetDatabase.LoadAssetAtPath(FileUtil.GetProjectRelativePath(path), typeof(Object));

            if (obj != null)
            {
                if (obj.GetType() == type)
                {
                    return obj;
                }
            }
        }

        return null;
    }
}
