using System.Collections;

using UnityEngine;
using UnityEditor;

public class MakeChildGameObject : ScriptableObject
{
    [MenuItem("Gameobject/Make Child Gameobject %#g", false, 0)]
    public static void MakeGameObject()
    {
        GameObject newObject = new GameObject("New Child");
        newObject.transform.parent = Selection.activeGameObject.transform;
        newObject.transform.position = Selection.activeGameObject.transform.position;
    }
}

