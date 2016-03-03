using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

	/// <summary>
	/// Title: Grendel Engine
	/// Author: Elliot Hudson
	/// Date: Apr 1, 2012
	/// 
	/// Filename: Info.cs
	/// 
	/// Summary: Tracks basics stats for use by other
	/// utilities (ie. Username, current session time, etc.
	/// 
	/// </summary>

public class InfoTracker : Singleton<InfoTracker> {

	private string _username = "";
	private string _machineName = "";	
	private string _unityVersion = "";
	
	private string _osPlatform = "";
	private string _osVersion = "";
	private string _osServicePack = "";
	
	private string _sysMemory = "";
	private string _processorCount = "";
	private string _processorType = "";
	
	private string _gpuName = "";
	private string _gpuMem = "";
	private string _gpuShader = "";
	
	//private float _currentPlaytime;
	
	public string Username
	{
		get {return _username;}
	}
	
	public string MachineName
	{
		get {return _machineName;}
	}
	
	public string ProcessorCount
	{
		get {return _processorCount;}
	}
	
	public string ProcessorType
	{
		get {return _processorType;}
	}
	
	public string UnityVersion
	{
		get {return _unityVersion;}
	}
	
	public string OSVersion
	{
		get {return _osVersion;}
	}
	
	public string OSPlatform
	{
		get {return _osPlatform;}
	}
	
	public string OSServicePack
	{
		get {return _osServicePack;}
	}
	
	public string GPUName
	{
		get {return _gpuName;}
	}
	
	public string GPUMem
	{
		get {return _gpuMem;}
	}
	
	public string GPUShader
	{
		get {return _gpuShader;}
	}
	
	public string SystemMemory
	{
		get {return _sysMemory;}
	}
	
	public string IsUnityEditor
	{
		get {return Application.isEditor.ToString();}
	}
	
	// Use this for initialization
	void Start () 
	{
		_username = Environment.UserName;
		_machineName = Environment.MachineName;
		_processorCount = Environment.ProcessorCount.ToString();
		_processorType = SystemInfo.processorType;
		_osPlatform = Environment.OSVersion.Platform.ToString();
		_osVersion = Environment.OSVersion.Version.ToString();
		_osServicePack = Environment.OSVersion.ServicePack;
		_gpuName = SystemInfo.graphicsDeviceName;
		_gpuMem = SystemInfo.graphicsMemorySize.ToString();
		_gpuShader = SystemInfo.graphicsShaderLevel.ToString();
		_sysMemory = SystemInfo.systemMemorySize.ToString();		
		_unityVersion = Application.unityVersion;		
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	
	}
}
