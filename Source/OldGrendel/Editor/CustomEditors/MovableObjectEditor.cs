using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MovableObjectController))]
public class MovableObjectEditor : GrendelEditor<MovableObjectController>
{	

    private const int kStatusIndicatorWidth = 128;

	public override void OnInspectorGUI()
	{
        base.OnInspectorGUI();

        if (Target.Positions == null || Target.Positions.Count <= 0)
        {

        }
        else
        {
            GUI.color = Color.white;

            int positionCount = 0;

            foreach(MovableObjectController.MovableObjectPosition position in Target.Positions)
            {
                EditorGUI.BeginChangeCheck();

                    EditorGUILayout.BeginVertical(GUI.skin.box);
                    EditorGUILayout.BeginHorizontal();

                        EditorGUILayout.LabelField(string.Format("Position {0}", (positionCount + 1).ToString()), EditorStyles.boldLabel);
    
                        if (Application.isPlaying)
                        {
                            if(Target.TargetObject.transform.position == Target.TargetObject.OriginalPosition + position.Position)
                            {
                                GUI.color = Color.green;
                                GUILayout.Box("At Position", GUILayout.Width(kStatusIndicatorWidth));
                                GUI.color = Color.white;
                            }
                            else if (Target.TargetObject.CurrentTargetPosition != null && Target.TargetObject.CurrentTargetPosition == position)
                            {
                                GUI.color = Color.yellow;
                                GUILayout.Box("Moving to Position", GUILayout.Width(kStatusIndicatorWidth));
                                GUI.color = Color.white;
                                Repaint();
                            }
                            else
                            {
                                if (GUILayout.Button("Move to position", GUILayout.Width(kStatusIndicatorWidth)))
                                {
                                    Target.TargetObject.MoveToPosition(Target.Positions[positionCount], Target.MoveSpeed);
                                }
                            }
                        }
    
                    EditorGUILayout.EndHorizontal();
                    position.Position = EditorGUILayout.Vector3Field("Position", position.Position);
                    position.Rotation = Quaternion.Euler(EditorGUILayout.Vector3Field("Rotation", position.Rotation.eulerAngles));
                    EditorGUILayout.Space();
                    EditorGUILayout.EndVertical();
                    positionCount++;

                if (EditorGUI.EndChangeCheck())
                {
                    EditorUtility.SetDirty(Target);
                }
            }
        }

        if (GUILayout.Button("Add Position"))
        {
            Target.Positions.Add(new MovableObjectController.MovableObjectPosition());
        }


	}

    private void OnSceneGUI()
    {        
		Undo.RecordObject(Target, "Moveable Object Change");

        int positionCount = 1;
        Vector3 originalPosition;

        if (Application.isEditor && !Application.isPlaying)
        {
            originalPosition = Target.TargetObject.transform.position;
        }
        else
        {
            originalPosition = Target.TargetObject.OriginalPosition;
        }

		Undo.RecordObject(Target, "Moveable Object Change");

        EditorGUI.BeginChangeCheck();

        foreach(MovableObjectController.MovableObjectPosition position in Target.Positions)
        {

            Handles.Label(originalPosition + position.Position, string.Format("Position {0}", positionCount.ToString()), EditorStyles.whiteLabel);
            position.Position = Handles.PositionHandle(originalPosition + position.Position, Quaternion.identity) - originalPosition;
            positionCount++;
        }

        if (EditorGUI.EndChangeCheck())
        {
			Undo.RecordObject(Target, "Moveable Object Change");
        }
    }

}

