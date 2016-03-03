using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

	/// <summary>
	/// Title: Grendel Engine
	/// Author: Elliot Hudson
	/// Date: Mar 12, 2012
	/// 
	/// Filename: ConsoleCommandRegistry.cs
	/// 
	/// Summary: Defines the ConsoleCommand Data Type
	/// and Handles the Registry of Console Commands,
	/// as well as adding commands to that registry ( via Register() )
	/// 
	/// </summary>

public class ConsoleCommandRegistry : Singleton<ConsoleCommandRegistry>
{	
	[System.Serializable]
	public struct ConsoleCommand
	{		
		public Action<ConsoleCommandParams> Method;
		public string Command;
		public string Description;
		public bool DebugOnly;
		
		public ConsoleCommand(Action<ConsoleCommandParams> method, string command, string description, bool debugOnly)
		{
			Method = method;
			Command = command;
			Description = description;
			DebugOnly = debugOnly;
		}
		
		public override string ToString()
        { 
            return string.Format("{0} - {1}{2}", Command, Description, (DebugOnly ? " [DEBUG ONLY]" : ""));
        }
	}
	
	private List<ConsoleCommand> _commandList = new List<ConsoleCommand>();
	private ConsoleCommandComparer _consoleCommandComparer = new ConsoleCommandComparer();
	
	public void Register(Action<ConsoleCommandParams> method, string command, string description, bool debugOnly)
	{		
		if (debugOnly && !GameManager.Instance.DebugBuild)
		{
			return;
		}
		
		ConsoleCommand consoleCommand = new ConsoleCommand(method, command, description, debugOnly);
		int index = _commandList.BinarySearch(consoleCommand, _consoleCommandComparer);
		if (index >= 0)
		{
				string before = _commandList[index].Command;
                string after = consoleCommand.Command;

                // Only warn if we're replacing with something different (different description, that is).
                if (before != after)
                {
                    Debug.LogWarning(string.Format("hotkey override: updating '{0}' with '{1}'", before, after));
                }

                _commandList[index] = consoleCommand;
		}
		else
		{
			_commandList.Insert(~index, consoleCommand);
		}
		
	}
	
	public IList<ConsoleCommand> RegisteredConsoleCommands
	{
		get { return _commandList.AsReadOnly(); }
	}
	
	public void HandleCommand(string command, ConsoleCommandParams parameters)
	{
		foreach(ConsoleCommand com in _commandList)
		{
			if(com.Command == command)
			{
				com.Method(parameters);
				return;
			}
		}
		
		Console.Instance.OutputToConsole(string.Format("There is no command '{0}'", command), Console.Instance.Style_Error);		
	}
	
	private class ConsoleCommandComparer: IComparer<ConsoleCommand>
    {
        // Compare by debuggability, then by command string
        public int Compare(ConsoleCommand x, ConsoleCommand y)
        {
            int diff = x.DebugOnly.CompareTo(y.DebugOnly);
            if (diff == 0)
            {
                diff = x.Command.CompareTo(y.Command);                   
            }

            return diff;
        }
    }

}//end class
