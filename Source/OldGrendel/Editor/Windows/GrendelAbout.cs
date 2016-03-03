using System.Collections;

using UnityEngine;
using UnityEditor;

public class GrendelAbout : EditorWindow
{
    private const string kAboutWindowTitle = "About Grendel";
    private static Rect kAboutPosition = new Rect(64f, 64f, 128f, 128f);
    private static Vector2 kAboutSize = new Vector2(192f, 256f);
    private const string kGrendelIconPath = "Assets/Third Party/Grendel/Textures/Grendel_Icon_Large_White.png";
    private static Rect kGrendelIconPosition = new Rect(16f, 16f, 128f, 128f);
    private static GrendelAbout mAboutWindowInstance;

    public static GrendelAbout AboutWindow
    {
        get
        {
            if (mAboutWindowInstance == null)
            {
                mAboutWindowInstance = (GrendelAbout)ScriptableObject.CreateInstance(typeof(GrendelAbout));
            }

            return mAboutWindowInstance;
        }
    }

    public static void OnDestroy()
    {
        mAboutWindowInstance = null;
    }
    
    [MenuItem ("Grendel/About Grendel...")]
    public static void Init()
    {             

        AboutWindow.name = kAboutWindowTitle;
        AboutWindow.title = kAboutWindowTitle;        
        AboutWindow.position = kAboutPosition;
        AboutWindow.minSize = kAboutSize;
        AboutWindow.maxSize = kAboutSize;
        AboutWindow.ShowUtility();
    }
    
    private void OnGUI()
    {
        GUI.color = Color.white;
        GUILayout.BeginHorizontal();
        
            GUILayout.BeginVertical();

                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                    GUILayout.Box((Texture)AssetDatabase.LoadAssetAtPath(kGrendelIconPath,typeof(Texture)), GUI.skin.label, new GUILayoutOption[]{GUILayout.Width(kGrendelIconPosition.width), GUILayout.Height(kGrendelIconPosition.height)});
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                    GUILayout.Label("Grendel Framework 1.01.1", EditorStyles.boldLabel);
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
        
                EditorGUILayout.Space();
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                    GUILayout.Label("by Elliot Hudson");
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();                
                
                EditorGUILayout.Space();
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                    GUILayout.Label("\u00A9 2013 Elliot Hudson");
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
        
                EditorGUILayout.Space();
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if(GUILayout.Button("www.ekhudson.com", GUI.skin.label))                
                {
                    Help.BrowseURL("http://www.ekhudson.com");
                }
        
                if(GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition))
                {
                    EditorGUIUtility.AddCursorRect(GUILayoutUtility.GetLastRect(), MouseCursor.Link);
                }
                
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
        
            GUILayout.EndVertical();
        
        GUILayout.EndHorizontal();
    }
    
}
