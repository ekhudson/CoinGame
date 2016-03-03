using UnityEngine;
using UnityEditor;

using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(ConsoleCommandRegistry))]
public class ConsoleCommandEditor : Editor 
{
	private IList<ConsoleCommandRegistry.ConsoleCommand> _commandList;
	
	virtual public ConsoleCommandRegistry Target
	{		
		get {return target as ConsoleCommandRegistry;}		
	}
	
	
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		
		_commandList = Target.RegisteredConsoleCommands;
		
		EditorGUILayout.LabelField(string.Format("Command Count: {0}", _commandList.Count));
		
		foreach(ConsoleCommandRegistry.ConsoleCommand command in _commandList)
		{
			EditorGUILayout.LabelField(string.Format("{0} : {1} {2}", command.Command, command.Description, command.DebugOnly ? "(Debug)" : ""));
		}
	}
}

