using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

	/// <summary>
	/// Title: Grendel Engine
	/// Author: Elliot Hudson
	/// Date: Apr 1, 2012
	/// 
	/// Filename: ConsoleLog.cs
	/// 
	/// Summary: Handles dumping the session's
	/// console output into a text file
	/// 
	/// </summary>

public static class ConsoleLog
{	
	
	public static void OutputLog(List<Console.ConsoleLine> commandList, string timeStamp)
	{
		
		bool errors = false;
		string filePath = Application.dataPath + "/ConsoleLogs/"; //build the path
		string fileName = timeStamp + ".txt"; //build the filename
		
		//Create the file and open it
		StreamWriter file = new StreamWriter(filePath + fileName);			
		
		//Verify that the file was created
		if (File.Exists(filePath + fileName))
		{
			Console.Instance.OutputToConsole("Log File " + fileName + " Created", Console.Instance.Style_Admin);
		}
		else
		{
			Console.Instance.OutputToConsole("Log File Could Not Be Created!", Console.Instance.Style_Error);
			return;
		}
		
		//Write Header Info
		file.WriteLine(System.String.Format("User Name: {0}", InfoTracker.Instance.Username));
		file.WriteLine(System.String.Format("Machine Name: {0}", InfoTracker.Instance.MachineName));
		file.WriteLine(System.String.Format("OS Platform: {0} - OS Version: {1} - OS Service Pack: {2}", InfoTracker.Instance.OSPlatform, InfoTracker.Instance.OSVersion, InfoTracker.Instance.OSServicePack));
		file.WriteLine(System.String.Format("Processor Type: {0} - Processor Count: {1}", InfoTracker.Instance.ProcessorType, InfoTracker.Instance.ProcessorCount));
		file.WriteLine(System.String.Format("System Memory: {0}", InfoTracker.Instance.SystemMemory));
		file.WriteLine(System.String.Format("GPU: {0} - GPU Memory: {1} - GPU Shader: {2}", InfoTracker.Instance.GPUName, InfoTracker.Instance.GPUMem, InfoTracker.Instance.GPUShader));
		file.WriteLine(System.String.Format("Unity Version: {0}", InfoTracker.Instance.UnityVersion));
		file.WriteLine(System.String.Format("Unity Editor: {0}", InfoTracker.Instance.IsUnityEditor));
		file.WriteLine(System.String.Format("\r\n" + "Total Playtime: {0} " + "\r\n", Time.realtimeSinceStartup));
		
		
		//Begin dumping the console contents
		foreach(Console.ConsoleLine command in Console.ConsoleLineList)
		{	
			try
			{
				file.WriteLine( System.String.Format( "{0} {1} - {2}", command.User, command.CommandDateTime, command.TextString) );
			}
			catch
			{
				errors = true;				
			}
		}
		
		//Close the file
		try
		{
			
			file.Close();
			
			if (errors)
			{
				Console.Instance.OutputToConsole("Log File Closed With Errors", Console.Instance.Style_Error);
			}
			else
			{
				Console.Instance.OutputToConsole("Log File " + fileName + " Closed", Console.Instance.Style_Admin);
			}
			
		}
		catch
		{
			Console.Instance.OutputToConsole("Log File Could Not Be Closed", Console.Instance.Style_Error);
		}		
	}	
}//end class
