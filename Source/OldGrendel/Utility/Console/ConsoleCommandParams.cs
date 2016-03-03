using UnityEngine;
using System.Collections;

	/// <summary>
	/// Title: Grendel Engine
	/// Author: Elliot Hudson
	/// Date: July 05, 2012
	/// 
	/// Filename: ConsoleCommandParams.cs
	/// 
	/// Summary: Allows for more easily passing parameters to ConsoleCommands.
	/// Has support for passing up to six parameters individually, or can
	/// accept an array of objects
	/// 
	/// </summary>

public class ConsoleCommandParams 
{
	public object[] Params;
		
	public ConsoleCommandParams()
	{
		//no parameters
	}
	
	public ConsoleCommandParams(object[] parameters)
	{		
		Params = new object[parameters.Length];
		Params = parameters;
	}
	
	public ConsoleCommandParams(object param01)
	{
		Params = new object[1];
		Params[0] = param01;
	}
	
	public ConsoleCommandParams(object param01, object param02)
	{
		Params = new object[2];
		Params[0] = param01;
		Params[1] = param02;
	}
	
	public ConsoleCommandParams(object param01, object param02, object param03)
	{
		Params = new object[3];
		Params[0] = param01;
		Params[1] = param02;
		Params[2] = param03;
	}
	
	public ConsoleCommandParams(object param01, object param02, object param03, object param04)
	{
		Params = new object[4];
		Params[0] = param01;
		Params[1] = param02;
		Params[2] = param03;
		Params[3] = param04;
	}
	
	public ConsoleCommandParams(object param01, object param02, object param03, object param04, object param05)
	{
		Params = new object[5];
		Params[0] = param01;
		Params[1] = param02;
		Params[2] = param03;
		Params[3] = param04;
		Params[4] = param05;
	}
	
	public ConsoleCommandParams(object param01, object param02, object param03, object param04, object param05, object param06)
	{
		Params = new object[6];
		Params[0] = param01;
		Params[1] = param02;
		Params[2] = param03;
		Params[3] = param04;
		Params[4] = param05;
		Params[5] = param06;
	}	
}
