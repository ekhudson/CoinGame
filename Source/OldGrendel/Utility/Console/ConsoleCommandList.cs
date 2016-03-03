using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

	/// <summary>
	/// Title: Grendel Engine
	/// Author: Elliot Hudson
	/// Date: July 05, 2012
	/// 
	/// Filename: ConsoleCommandParams.cs
	/// 
	/// Summary: All of the Console Commands for the game.
	/// These commands are registered with the ConsoleCommandRegistry
	/// on Start (this class must exist in the scene)
	/// 
	/// </summary>

public class ConsoleCommandList : Singleton<ConsoleCommandList> 
{	
	void Start()
	{		
		RegisterCommands();
	}
	
	//Register all console commands here. Place their methods below.
	void RegisterCommands()
	{
		ConsoleCommandRegistry.Instance.Register(Stats, "Stats", "Display stats on the screen", true);
		ConsoleCommandRegistry.Instance.Register(FPS, "FPS", "Display FPS on the screen", true);
		ConsoleCommandRegistry.Instance.Register(Quit, "Quit", "Quit the Game", true);
		ConsoleCommandRegistry.Instance.Register(NextTrack, "NextTrack", "Go to next music track", true);
		ConsoleCommandRegistry.Instance.Register(PreviousTrack, "PreviousTrack", "Go to previous music track", true);
		ConsoleCommandRegistry.Instance.Register(PlayMusicTrack, "PlayMusicTrack", "Go to track #", true);
		ConsoleCommandRegistry.Instance.Register(RandomTrack, "RandomTrack", "Go to a random music track", true);
		ConsoleCommandRegistry.Instance.Register(ListMusicTracks, "ListMusicTracks", "List all music tracks", true);
		ConsoleCommandRegistry.Instance.Register(OutputLog, "OutputLog", "Dump the console log to a .txt file", true);
		ConsoleCommandRegistry.Instance.Register(Test, "Test", "Test command", true);
		ConsoleCommandRegistry.Instance.Register(ShowCommands, "ShowCommands", "List all Console Commands", true);		
	}	

	void Quit(ConsoleCommandParams parameters)
	{		
		Console.Instance.OutputToConsole("Quitting " + Application.absoluteURL, Console.Instance.Style_Admin);
		Application.Quit();
	}	
	
	public void FPS(ConsoleCommandParams parameters)
	{		
		if( FPSCounter.Instance.ShowFPS ) 
		{ 
			FPSCounter.Instance.ShowFPS = false;
			Console.Instance.OutputToConsole("FPS Counter Disabled" + Application.absoluteURL, Console.Instance.Style_Admin);
		} 
		else 
		{ 
			FPSCounter.Instance.ShowFPS = true;
			Console.Instance.OutputToConsole("FPS Counter Enabled" + Application.absoluteURL, Console.Instance.Style_Admin);
		}
	}	
	
	public void Stats(ConsoleCommandParams parameters)
	{
		FPS(parameters);
	}
	
	void NextTrack(ConsoleCommandParams parameters)
	{
		Console.Instance.OutputToConsole("Go to next music track.", Console.Instance.Style_Admin);
		//AudioManager.Instance.IncrementMusicTrack(1);		
	}
	
	void PreviousTrack(ConsoleCommandParams parameters)
	{
		Console.Instance.OutputToConsole("Go to previous music track.", Console.Instance.Style_Admin);
		//AudioManager.Instance.IncrementMusicTrack(-1);		
	}
	
	void RandomTrack(ConsoleCommandParams parameters)
	{
		//int randomNum = UnityEngine.Random.Range(0, AudioList.Instance.MusicTracks.Count);
		//Console.Instance.OutputToConsole(string.Format("Going to a random track: {0} ", AudioList.Instance.MusicTracks[randomNum].ToString()), Console.Instance.Style_Admin);
		//AudioManager.Instance.PlayMusicTrack(AudioList.Instance.MusicTracks[randomNum]);	
	}
	
	void PlayMusicTrack(ConsoleCommandParams parameters)
	{		
		
		if (parameters.Params == null || parameters.Params.Length == 0 || parameters.Params.Length > 1)
		{
			Console.Instance.OutputToConsole(string.Format("Incorrect Parameters: {0}", parameters.Params.ToString()) , Console.Instance.Style_Error);
		}
		else
		{
			//int index = (int)parameters.Params[0];
			//Console.Instance.OutputToConsole(string.Format("Requesting music track {0} : {1}", index.ToString(), AudioList.Instance.MusicTracks[index].ToString()), Console.Instance.Style_Admin);
		}
	}
	
	void ListMusicTracks(ConsoleCommandParams parameters)
	{
		Console.Instance.OutputToConsole("Listing Music Tracks: ", Console.Instance.Style_Admin);
		Console.Instance.OutputToConsole("", Console.Instance.Style_Admin, false);
		
//		for(int i = 0; i < AudioList.Instance.MusicTracks.Count; i++)
//		{
//			Console.Instance.OutputToConsole( string.Format("{0} : {1}", i.ToString(), AudioList.Instance.MusicTracks[i]), Console.Instance.Style_Admin, false);			
//		}
		
		Console.Instance.OutputToConsole("", Console.Instance.Style_Admin, false);
	}
	
	void OutputLog(ConsoleCommandParams parameters)
	{		
		ConsoleLog.OutputLog(Console.ConsoleLineList, System.DateTime.Now.ToString("yymmdd HHmmss"));	
	}
	
	void ShowCommands(ConsoleCommandParams parameters)
	{		
		Console.Instance.OutputToConsole("Listing Available Commands: ", Console.Instance.Style_Admin);
		Console.Instance.OutputToConsole(string.Empty, Console.Instance.Style_Admin, false);
		
		foreach(ConsoleCommandRegistry.ConsoleCommand command in ConsoleCommandRegistry.Instance.RegisteredConsoleCommands)		
		{
			Console.Instance.OutputToConsole(command.Command, Console.Instance.Style_UserPrevious, false, false);
			Console.Instance.OutputToConsole(command.ToString(), Console.Instance.Style_Admin, false, false);
		}
		
		Console.Instance.OutputToConsole("", Console.Instance.Style_Admin, false);
	}
	
	void Test(ConsoleCommandParams parameters)
	{		
		if (parameters.Params == null || parameters.Params[0] == null)
		{
			Console.Instance.OutputToConsole("Parameter missing", Console.Instance.Style_Error);
		}
		else
		{
			Console.Instance.OutputToConsole("Success!", Console.Instance.Style_Admin);
		}
	}
	
	
	
					
//		//QUIT
//		if (splitCommand[0] == "Quit" || splitCommand[0] == "quit")
//		{
//			Application.Quit();	
//		}
//		
//		//GOD MODE
//		if (splitCommand[0] == "God" || splitCommand[0] == "god")
//		{
//			
//			if (splitCommand.Length > 1 && splitCommand[1] == "1")
//			{
//				GameManager.PlayerRef.PlayerShip.GodMode = true;
//				responseStyle = Style_Admin;
//				responseText = "God Mode Enabled";
//			}
//			else if (splitCommand.Length > 1 && splitCommand[1] == "0")
//			{
//				GameManager.PlayerRef.PlayerShip.GodMode = false;
//				responseStyle = Style_Admin;
//				responseText = "God Mode Disabled";
//			}
//			else
//			{
//				if (!GameManager.PlayerRef.PlayerShip.GodMode)
//				{
//					GameManager.PlayerRef.PlayerShip.GodMode = true;
//					responseStyle = Style_Admin;
//					responseText = "God Mode Enabled";
//				}
//				else
//				{
//					GameManager.PlayerRef.PlayerShip.GodMode = false;
//					responseStyle = Style_Admin;
//					responseText = "God Mode Disabled";
//				}
//			}			
//		}
//		
//		//UNLOCK CAMERA
//		if (splitCommand[0] == "UnlockCam" || splitCommand[0] == "unlockcam")
//		{
//			
//			if (splitCommand.Length > 1 && splitCommand[1] == "1")
//			{
//				GameManager.CameraRef.UnlockedCam = true;
//				responseStyle = Style_Admin;
//				responseText = "Camera Unlocked";
//			}
//			else if (splitCommand.Length > 1 && splitCommand[1] == "0")
//			{
//				GameManager.CameraRef.UnlockedCam = false;
//				responseStyle = Style_Admin;
//				responseText = "Camera Locked";
//			}
//			else
//			{
//				if (!GameManager.CameraRef.UnlockedCam)
//				{
//					GameManager.CameraRef.UnlockedCam = true;
//					responseStyle = Style_Admin;
//					responseText = "Camera Unlocked";
//				}
//				else
//				{
//					GameManager.CameraRef.UnlockedCam = false;
//					responseStyle = Style_Admin;
//					responseText = "Camera Locked";
//				}
//			}			
//		}
//		
//		
//		
//		//LOAD LEVEL
//		if (splitCommand[0] == "LoadLevel" || splitCommand[0] == "loadlevel")
//		{
//			
//			if (splitCommand.Length > 1)
//			{
//				responseStyle = Style_Admin;
//				responseText = "LoadLevel " + splitCommand[1];
//				GameManager.LoadLevel(splitCommand[1]);
//			}
//			else
//			{
//				responseStyle = Style_Error;
//				responseText = "Scene name not specified. Command syntax is 'LoadLevel SceneName'";
//			}                      
//			
//		}
//		
//		
//		
//		//RESTART LEVEL
//		if (splitCommand[0] == "RestartLevel" || splitCommand[0] == "restartlevel")
//		{
//			
//			if (splitCommand.Length > 1)
//			{
//				responseStyle = Style_Error;
//				responseText = "Unwanted parameter: " + splitCommand[1] + ". RestartLevel does not require any parameters";
//			}
//			else
//			{
//				responseStyle = Style_Admin;
//				responseText = "Restarting level.";
//				GameManager.LoadLevel(Application.loadedLevelName);
//			}                      
//			
//		}
//		
//		//MUTE ALL
//		if (splitCommand[0] == "MuteAll" || splitCommand[0] == "muteall")
//		{
//			AudioManager.GlobalVolume = 0;			
//			responseStyle = Style_Admin;
//			responseText = "All sounds muted";
//		}
//		
//		//INFINITE ENERGY
//		if (splitCommand[0] == "Energy" || splitCommand[0] == "energy")
//		{
//			
//			if (splitCommand.Length > 1 && splitCommand[1] == "1")
//			{
//				GameManager.PlayerRef.InfiniteEnergy = true;
//				responseStyle = Style_Admin;
//				responseText = "Infinite Energy";
//			}
//			else if (splitCommand.Length > 1 && splitCommand[1] == "0")
//			{
//				GameManager.PlayerRef.InfiniteEnergy = false;
//				responseStyle = Style_Admin;
//				responseText = "Regular Energy";
//			}
//			else
//			{
//				if (!GameManager.PlayerRef.InfiniteEnergy)
//				{
//					GameManager.PlayerRef.InfiniteEnergy = true;
//					responseStyle = Style_Admin;
//					responseText = "Infinite Energy";
//				}
//				else
//				{
//					GameManager.PlayerRef.InfiniteEnergy = false;
//					responseStyle = Style_Admin;
//					responseText = "Regular Energy";
//				}
//			}			
//		}
//		
//		//INFINITE SLOWMO
//		if (splitCommand[0] == "Slowmo" || splitCommand[0] == "slowmo")
//		{
//			
//			if (splitCommand.Length > 1 && splitCommand[1] == "1")
//			{
//				GameManager.PlayerRef.InfiniteSlomo = true;
//				responseStyle = Style_Admin;
//				responseText = "Infinite Slowmo";
//			}
//			else if (splitCommand.Length > 1 && splitCommand[1] == "0")
//			{
//				GameManager.PlayerRef.InfiniteSlomo = false;
//				responseStyle = Style_Admin;
//				responseText = "Regular Slowmo";
//			}
//			else
//			{
//				if (!GameManager.PlayerRef.InfiniteSlomo)
//				{
//					GameManager.PlayerRef.InfiniteSlomo = true;
//					responseStyle = Style_Admin;
//					responseText = "Infinite Slowmo";
//				}
//				else
//				{
//					GameManager.PlayerRef.InfiniteSlomo = false;
//					responseStyle = Style_Admin;
//					responseText = "Regular Slowmo";
//				}
//			}			
//		}
//		
//		//INFINITE EVERYTHING
//		if (splitCommand[0] == "Impulse" || splitCommand[0] == "impulse")
//		{
//			
//			if (splitCommand.Length > 1 && splitCommand[1] == "1")
//			{
//				GameManager.PlayerRef.PlayerShip.GodMode = true;
//				GameManager.PlayerRef.InfiniteEnergy = true;
//				GameManager.PlayerRef.InfiniteSlomo = true;
//				responseStyle = Style_Admin;
//				responseText = "Infinite Slowmo, Infinite Energy, God Mode Enabled";
//			}
//			else if (splitCommand.Length > 1 && splitCommand[1] == "0")
//			{
//				GameManager.PlayerRef.PlayerShip.GodMode = false;
//				GameManager.PlayerRef.InfiniteEnergy = false;
//				GameManager.PlayerRef.InfiniteSlomo = false;
//				responseStyle = Style_Admin;
//				responseText = "Regular Slowmo";
//			}
//			else
//			{
//				responseStyle = Style_Error;
//				responseText = "Missing parameter. Correct syntax is 'Impulse [0]' or 'Impulse [1]'";
//			}			
//		}
//		
//		// FILL OVERCHARGE BAR
//		if (splitCommand[0] == "FullOvercharge" || splitCommand[0] == "fullovercharge" || splitCommand[0] == "fullOvercharge")
//		{
//			GameManager.PlayerRef.CurrentOvercharge = GameManager.PlayerRef.MaxOvercharge; // Max overcharge
//			responseStyle = Style_Admin;
//			responseText = "Overcharge meter filled to full.";
//		}
//		
//		//SPAWN OBJECT
//		if (splitCommand[0] == "Spawn" || splitCommand[0] == "spawn")
//		{
//			
//			if (splitCommand.Length > 1)
//			{
//				int count = 1;
//				float spread = 1f;
//				Vector3 location = GameManager.PlayerRef.Position;
//				
//				if (splitCommand.Length > 2)
//				{
//					count = System.Convert.ToInt32(splitCommand[2]);
//					spread *= count;
//				}
//				
//				responseText = "";
//				
//				for (int i = 0; i < count; i++)
//				{						
//					Vector3 tempLoc = new Vector3(location.x + Random.Range(-spread, spread), 0, location.z + Random.Range(-spread, spread));
//					GameObject SpawnedObject = (GameObject)Instantiate(Resources.Load("Prefabs/" + splitCommand[1]), tempLoc, Quaternion.identity);
//					responseStyle = Style_Admin;
//					responseText += splitCommand[1] + " spawned @ " + tempLoc + " | ";
//				}
//			}
//			else
//			{
//				responseStyle = Style_Error;
//				responseText = "Missing parameter. Correct syntax is 'Spawn [prefab path/name] [count]'";
//			}
//		}
//		
//		//TOGGLE FPS
//		if (splitCommand[0] == "ShowFPS" || splitCommand[0] == "showfps")
//		{
//			
//			if (_bShowFPS != true)
//			{
//				_bShowFPS = true;
//				responseStyle = Style_Admin;
//				responseText = "Showing FPS";
//			}
//			else
//			{
//				_bShowFPS = false;
//				responseStyle = Style_Admin;
//				responseText = "Hiding FPS";
//			}
//		}
//		
//		//Adjust Bullet Speed
//		if (splitCommand[0] == "BulletSpeed" || splitCommand[0] == "bulletspeed")
//		{
//			
//			if (splitCommand.Length > 1)
//			{				
//				GameManager.GlobalBulletSpeedModifier = float.Parse(splitCommand[1]);
//				responseStyle = Style_Admin;
//				responseText = "Global Bullet Speed Modifier set to: " + splitCommand[1];
//			}			
//			else
//			{
//				responseStyle = Style_Error;
//				responseText = "Missing parameter. Correct syntax is 'BulletSpeed [float Multiplier]'";
//			}			
//		}
//		
//		//Adjust Global Damage
//		if (splitCommand[0] == "GlobalDamage" || splitCommand[0] == "globaldamage")
//		{
//			
//			if (splitCommand.Length > 1)
//			{				
//				GameManager.GlobalDamageModifier = float.Parse(splitCommand[1]);
//				responseStyle = Style_Admin;
//				responseText = "Global Damage Modifier set to: " + splitCommand[1];
//			}			
//			else
//			{
//				responseStyle = Style_Error;
//				responseText = "Missing parameter. Correct syntax is 'GlobalDamage [float Multiplier]'";
//			}			
//		}
//		
//		//Toggle Rick Mode
////		if (splitCommand[0] == "RickMode" || splitCommand[0] == "rickmode")
////		{
////			
////			if (GameManager.GlobalBulletSpeedModifier == 1)
////			{				
////				GameManager.CameraRef.RickMode = true;
////				AudioManager.GotoLuciMusic();
////				GameManager.GlobalBulletSpeedModifier = 0.25f;
////				GameManager.GlobalDamageModifier = 5;
////				responseStyle = Style_Admin;
////				responseText = "RickMode Activated. All glory to Her Majesty's Jewel of the South, the Commonwealth of Australia.";
////			}			
////			else
////			{
////				GameManager.CameraRef.RickMode = false;
////				AudioManager.GotoDefaultMusic();
////				GameManager.GlobalBulletSpeedModifier = 1;
////				GameManager.GlobalDamageModifier = 1;
////				responseStyle = Style_Admin;
////				responseText = "RickMode Deactivated. Not my bowl of rice.";
////			}			
////		}
////		
////		//Toggle NyanMode
////		if (splitCommand[0] == "NyanMode" || splitCommand[0] == "nyanmode")
////		{
////			if (!GameManager.NyanMode)
////			{
////				GameManager.NyanMode = true;
////				GameManager.PlayerRef.Sprite.PlayAnim(4);
////				AudioManager.PlayUnmanagedSound(GameManager.LevelManagerRef.NyanMusic, 2.0f, true);
////				responseStyle = Style_Admin;
////				responseText = "NYAN! NYAN! NYAN!";
////			}
////			else
////			{
////				GameManager.NyanMode = false;
////				GameManager.PlayerRef.Sprite.PlayAnim(1);
////				responseStyle = Style_Admin;
////				responseText = "NyanMode deactivated";
////			}
////		}
////		
////		//Toggle NyanMode
////		if (splitCommand[0] == "DragonMode" || splitCommand[0] == "dragonmode")
////		{
////			if (!GameManager.DragonMode)
////			{
////				GameManager.DragonMode = true;						
////			
////				AudioManager.PlayCombatMusic(GameManager.LevelManagerRef.DragonMusic, 2, true);
////				AudioManager.BGMusicAudioSource.Stop();
////				
////				for (int i = 0; i < 100; i++)
////				{					
////					Vector3 tempLoc = new Vector3(GameManager.PlayerRef.Position.x + Random.Range(-1000, 1000), 0, GameManager.PlayerRef.Position.z + Random.Range(-1000, 1000));
////					GameObject SpawnedObject = (GameObject)Instantiate(Resources.Load("Prefabs/Entities/Dragon"), tempLoc, Quaternion.identity);
////				}
////				
////				responseStyle = Style_Admin;
////				responseText = "You are the last dragon.";
////			}
////			else
////			{
////				GameManager.DragonMode = false;				
////				responseStyle = Style_Admin;
////				responseText = "You took an arrow to the knee...";
////			}
////		}
//		
//		//Toggle HotSpawning
//		if (splitCommand[0] == "HotSpawn" || splitCommand[0] == "hotspawn")
//		{
//			if (!_bHotSpawnEnabled)
//			{				
//				_bHotSpawnEnabled = true;
//				responseStyle = Style_Admin;
//				responseText = "Hot Spawning Enabled";
//			}
//			else
//			{				
//				_bHotSpawnEnabled = false;
//				responseStyle = Style_Admin;
//				responseText = "Hot Spawning Disabled";
//			}
//		}
//		
//		//Toggle DebugDrones
//		if (splitCommand[0] == "DebugDrones" || splitCommand[0] == "debugdrones")
//		{
//			if (!GameManager.UserInputRef.EnableDebugDrones)
//			{				
//				GameManager.UserInputRef.EnableDebugDrones = true;
//				responseStyle = Style_Admin;
//				responseText = "Debug Drones Enabled";
//			}
//			else
//			{				
//				GameManager.UserInputRef.EnableDebugDrones = false;
//				responseStyle = Style_Admin;
//				responseText = "Debug Drones Disabled";
//			}
//		}
//		
//		//Toggle HotSpawning
//		if (splitCommand[0] == "Zoom" || splitCommand[0] == "zoom")
//		{
//			if (!_bCameraScrollEnabled)
//			{				
//				_bCameraScrollEnabled = true;
//				responseStyle = Style_Admin;
//				responseText = "Camera Scrolling Enabled";
//			}
//			else
//			{				
//				_bCameraScrollEnabled = false;
//				responseStyle = Style_Admin;
//				responseText = "Camera Scrolling Disabled";
//			}
//		}
//		
//		//DISABLE/ENABLE IMAGE EFFECTS
//		if (splitCommand[0] == "ImageEffects" || splitCommand[0] == "imageeffects")
//		{
//			
//			if (splitCommand.Length > 1 && splitCommand[1] == "1")
//			{
//				GameManager.DisableImageEffects = false;
//				GameManager.CheckQualitySettings();
//				responseStyle = Style_Admin;
//				responseText = "Image Effects Enabled";
//			}
//			else if (splitCommand.Length > 1 && splitCommand[1] == "0")
//			{
//				GameManager.DisableImageEffects = true;
//				GameManager.CheckQualitySettings();
//				responseStyle = Style_Admin;
//				responseText = "Image Effects Disabled";
//			}
//			else
//			{
//				responseStyle = Style_Error;
//				responseText = "Missing parameter. Correct syntax is 'ImageEffects [1 = On / 0 = Off]'";
//			}			
//		}
//		
//			
//		//DISABLE/ENABLE PARTICLES
//		if (splitCommand[0] == "Particles" || splitCommand[0] == "particles")
//		{
//			
//			if (splitCommand.Length > 1 && splitCommand[1] == "1")
//			{
//				GameManager.DisableParticles = false;
//				GameManager.CheckQualitySettings();
//				responseStyle = Style_Admin;
//				responseText = "Particles Enabled";
//			}
//			else if (splitCommand.Length > 1 && splitCommand[1] == "0")
//			{
//				GameManager.DisableParticles = true;
//				GameManager.CheckQualitySettings();
//				responseStyle = Style_Admin;
//				responseText = "Particles Disabled";
//			}
//			else
//			{
//				responseStyle = Style_Error;
//				responseText = "Missing parameter. Correct syntax is 'Particles [1 = On / 0 = Off]'";
//			}			
//		}
//		
//		//DISABLE/ENABLE PARTICLES
//		if (splitCommand[0] == "JetStreams" || splitCommand[0] == "jetstreams")
//		{
//			
//			if (splitCommand.Length > 1 && splitCommand[1] == "1")
//			{
//				GameManager.DisableJetStreams = false;
//				GameManager.CheckQualitySettings();
//				responseStyle = Style_Admin;
//				responseText = "JetStreams Enabled";
//			}
//			else if (splitCommand.Length > 1 && splitCommand[1] == "0")
//			{
//				GameManager.DisableJetStreams = true;				
//				GameManager.CheckQualitySettings();
//				responseStyle = Style_Admin;
//				responseText = "JetStreams Disabled";
//			}
//			else
//			{
//				responseStyle = Style_Error;
//				responseText = "Missing parameter. Correct syntax is 'JetStreams [1 = On / 0 = Off]'";
//			}			
//		}
//		
//		
//		//Ortho Size
//		if (splitCommand[0] == "Orthosize" || splitCommand[0] == "orthosize")
//		{
//			
//			if (splitCommand.Length > 1)
//			{				
//				Camera.main.orthographicSize = float.Parse(splitCommand[1]);
//				responseStyle = Style_Admin;
//				responseText = "Camera Orthographic Size Set To: " + splitCommand[1];
//			}			
//			else
//			{
//				responseStyle = Style_Error;
//				responseText = "Missing parameter. Correct syntax is 'Orthosize [float Orthographic Size]'";
//			}			
//		}		
//				
//		ConsoleLine line = new ConsoleLine(); //create an empty console line object
//		line.Style = responseStyle; //set the Style
//		line.TextString = responseText; //set the text
//		ConsoleCommandList.Add(line); //add the line to the console output		
//	
	//}//end RunCommand
	
}
