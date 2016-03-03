using UnityEngine;
using System.Collections;

	/// <summary>
	/// Title: Grendel Engine
	/// Author: Elliot Hudson
	/// Date: Feb 15, 2012
	/// 
	/// Filename: GameManager.cs
	/// 
	/// Summary: Essentially holds information that might need to be
	/// accessed by a variety of objects in the scene, and contains
	/// global references to useful scripts (ie. UserInput, EntityManager)
	///  
	/// </summary>

public class GameManager : Singleton<GameManager>
{
    #region PUBLIC VARIABLES
    public string ApplicationTitle = "Grendel";
    public string ApplicationVersion = "1.0";
    public bool DebugBuild = true;

    [SerializeField]public UnityEngine.Object SceneToLoadOnGameLaunch;
    [SerializeField]public UnityEngine.Object MainMenuScene;
	
	private static bool mFirstTimeLoaded = true; //is this the very first time this script was loaded?
    [HideInInspector]public bool LoadDefaultScene; //are we supposed to load the default scene?
    
	protected GrendelProjectData mProjectData;
	
	public GrendelProjectData ProjectData
	{
		get
		{
			if (mProjectData == null)
			{
				GetProjectData();
			}
			
			return mProjectData;
		}
	}
	
	private void GetProjectData()
	{
		mProjectData = (GrendelProjectData)Resources.Load("Grendel/Grendel Project Data", typeof(GrendelProjectData));
		
		if (mProjectData == null)
		{
			Debug.LogError("This Grendel Project is missing its project data. Please make sure there is one located at Assets|Resource|Grendel", this);
		}
	}

    public static class GameStates
    {
        public enum STATES
        {
             LOADING,
             INTRO,
             MAINMENU,
             OPTIONS,
             RUNNING,
             PAUSED,
             CREDITS,
             BOOTUP, //only called when the game is first started
        }
    }
    #endregion
    
    #region PRIVATE VARIABLES
    
    private GameStates.STATES mGameState;
    
    #endregion
    
    #region PROPERTIES
    public GameStates.STATES GameState
    {
        get{return mGameState;}
    }
    #endregion

    protected override void Awake()
    {
        StartCoroutine("AwakeCheck");
        base.Awake();
    }

    // Use this for initialization
    protected virtual void Start ()
    {

    }

    protected virtual void Update()
    {
        switch (mGameState)
        {
            case GameStates.STATES.LOADING:
    
            if (ComponentsLoaded())
            {
                if (MainMenuScene != null && Application.loadedLevelName == MainMenuScene.name)
                {
                    SetGameState(GameStates.STATES.MAINMENU);
                }
                else
                {
                    SetGameState(GameStates.STATES.RUNNING);
                }
            }
         
            break;

            case GameStates.STATES.INTRO:
    
            break;
    
            case GameStates.STATES.MAINMENU:
    
            break;
    
            case GameStates.STATES.OPTIONS:
    
            break;
    
            case GameStates.STATES.RUNNING:
    
            break;
    
            case GameStates.STATES.PAUSED:
    
            break;
    
            case GameStates.STATES.CREDITS:

            break;
    
            case GameStates.STATES.BOOTUP:

            break;
    
            default:
    
            break;
        }
    }

    public void SetGameState(GameStates.STATES state)
    {
        if (state == mGameState)
        {
            Console.Instance.OutputToConsole(string.Format("{0}: Attempted to set GameState to {1}, but GameState is already set to {2}", this.ToString(), state.ToString(), mGameState.ToString()), Console.Instance.Style_Admin);
            return;
        }
     
        switch(state)
        {
            case GameStates.STATES.LOADING:
    
                switch (mGameState)
                {
                    case GameStates.STATES.LOADING:
    
                    break;
    
                    case GameStates.STATES.INTRO:
    
                    break;
    
                    case GameStates.STATES.MAINMENU:
        
                    break;
        
                    case GameStates.STATES.OPTIONS:
    
                    break;
        
                    case GameStates.STATES.RUNNING:
        
                    break;
    
                    case GameStates.STATES.PAUSED:
        
                    break;
        
                    case GameStates.STATES.CREDITS:
        
                    break;
    
                    case GameStates.STATES.BOOTUP:
        
                    break;
    
                    default:
    
                    break;
                }
            
            break;
    
            case GameStates.STATES.INTRO:
        
                switch (mGameState)
                {
                    case GameStates.STATES.LOADING:
    
                    break;
        
                    case GameStates.STATES.INTRO:
        
                    break;
    
                    case GameStates.STATES.MAINMENU:
            
                    break;
            
                    case GameStates.STATES.OPTIONS:
                
                    break;
            
                    case GameStates.STATES.RUNNING:
    
                    break;
        
                    case GameStates.STATES.PAUSED:
            
                    break;
    
                    case GameStates.STATES.CREDITS:
        
                    break;
        
                    case GameStates.STATES.BOOTUP:
        
                    break;
        
                    default:
        
                    break;
                }
        
            break;
    
            case GameStates.STATES.MAINMENU:
            
                switch (mGameState)
                {
                    case GameStates.STATES.LOADING:
        
                    break;
        
                    case GameStates.STATES.INTRO:
    
                    LevelManager.Instance.LoadLevel(MainMenuScene.ToString());
            
                    break;
                    
                    case GameStates.STATES.MAINMENU:
    
                    break;
        
                    case GameStates.STATES.OPTIONS:
        
                    break;
    
                    case GameStates.STATES.RUNNING:
        
                    LevelManager.Instance.LoadLevel(MainMenuScene.ToString());
        
                    break;
        
                    case GameStates.STATES.PAUSED:
        
                    break;
        
                    case GameStates.STATES.CREDITS:
    
                    LevelManager.Instance.LoadLevel(MainMenuScene.ToString());
        
                    break;
        
                    case GameStates.STATES.BOOTUP:
        
                    break;
    
                    default:
        
                    break;
                }
            
            break;
         
            case GameStates.STATES.OPTIONS:
    
                switch (mGameState)
                {
                    case GameStates.STATES.LOADING:
        
                    break;
        
                    case GameStates.STATES.INTRO:
        
                    break;
        
                    case GameStates.STATES.MAINMENU:
        
                    break;
        
                    case GameStates.STATES.OPTIONS:
        
                    break;
        
                    case GameStates.STATES.RUNNING:
        
                    break;
        
                    case GameStates.STATES.PAUSED:
    
                    break;
        
                    case GameStates.STATES.CREDITS:
        
                    break;
        
                    case GameStates.STATES.BOOTUP:
        
                    break;
        
                    default:
    
                    break;
                }
            
            break;
         
            case GameStates.STATES.RUNNING:
    
                switch (mGameState)
                {
                    case GameStates.STATES.LOADING:
        
                    break;
    
                    case GameStates.STATES.INTRO:
        
                    break;
        
                    case GameStates.STATES.MAINMENU:
    
                    break;
            
                    case GameStates.STATES.OPTIONS:
        
                    break;
        
                    case GameStates.STATES.RUNNING:
        
                    break;
        
                    case GameStates.STATES.PAUSED:
        
                    break;
        
                    case GameStates.STATES.CREDITS:
        
                    break;
        
                    case GameStates.STATES.BOOTUP:
        
                    break;
        
                    default:
        
                    break;
                }
            
            break;
    
            case GameStates.STATES.PAUSED:
            
                switch (mGameState)
                {
                    case GameStates.STATES.LOADING:
    
                    break;
        
                    case GameStates.STATES.INTRO:
            
                    break;
            
                    case GameStates.STATES.MAINMENU:
            
                    break;
            
                    case GameStates.STATES.OPTIONS:
            
                    break;
            
                    case GameStates.STATES.RUNNING:
                    
                    break;
        
                    case GameStates.STATES.PAUSED:
        
                    break;
        
                    case GameStates.STATES.CREDITS:
        
                    break;
        
                    case GameStates.STATES.BOOTUP:
        
                    break;
        
                    default:
    
                    break;
                }
            
            break;
         
            case GameStates.STATES.CREDITS:
            
                switch (mGameState)
                {
                    case GameStates.STATES.LOADING:
        
                    break;
        
                    case GameStates.STATES.INTRO:
        
                    break;
    
                    case GameStates.STATES.MAINMENU:
        
                    break;
        
                    case GameStates.STATES.OPTIONS:
        
                    break;
        
                    case GameStates.STATES.RUNNING:
        
                    break;
        
                    case GameStates.STATES.PAUSED:
        
                    break;
        
                    case GameStates.STATES.CREDITS:
        
                    break;
    
                    case GameStates.STATES.BOOTUP:
    
                    break;
        
                    default:
        
                    break;
                }
    
            break;
        
            case GameStates.STATES.BOOTUP:
            
                switch (mGameState)
                {
                    case GameStates.STATES.LOADING:
        
                    break;
            
                    case GameStates.STATES.INTRO:
            
                    break;
        
                    case GameStates.STATES.MAINMENU:
    
                    break;
            
                    case GameStates.STATES.OPTIONS:
        
                    break;
    
                    case GameStates.STATES.RUNNING:
        
                    break;
        
                    case GameStates.STATES.PAUSED:
    
                    break;
        
                    case GameStates.STATES.CREDITS:
        
                    break;
    
                    case GameStates.STATES.BOOTUP:
        
                    break;
        
                    default:
    
                    break;
                }
    
                break;
    
         default:
    
         break;
    
        }

        Console.Instance.OutputToConsole(string.Format("{0}: Setting GameState to {1}. Previous state: {2}", this.ToString(), state.ToString(), mGameState.ToString()), Console.Instance.Style_Admin);
        EventManager.Instance.Post(new GameStateEvent(state, mGameState, this));
        mGameState = state;
    }

    private void BootupSequence()
    {
        LaunchDefaultStartScene();
    }

    private void LaunchDefaultStartScene()
    {
        if(mGameState == GameStates.STATES.BOOTUP && SceneToLoadOnGameLaunch != null && Application.loadedLevelName != SceneToLoadOnGameLaunch.name && LoadDefaultScene)
        {
            Console.Instance.OutputToConsole(string.Format("{0}: Supposed to start up in Scene {1}, but this is Scene {2}",  this.ToString(), SceneToLoadOnGameLaunch.name, Application.loadedLevel), Console.Instance.Style_Admin);
          
            SetGameState(GameStates.STATES.LOADING);
            LevelManager.Instance.LoadLevel(SceneToLoadOnGameLaunch.name);
        }
		else if (mGameState == GameStates.STATES.BOOTUP)
		{
			SetGameState(GameStates.STATES.LOADING);
		}
    }

    private bool ComponentsLoaded()
    {       
        object[] singletons = FindObjectsOfType(typeof(Singleton<>));
    
        foreach(object st in singletons)
        {
            if (st == null)
            {
                return false;
            }
        }

        return true;
    }

    IEnumerator AwakeCheck()
    {
        if (mFirstTimeLoaded)
        {
            //TODO: These should timeout to avoid infinite loops!!
			while(Console.Instance == null)
            {
				yield return new WaitForSeconds(Time.deltaTime);
            }

			while(EventManager.Instance == null)
			{
				yield return new WaitForSeconds(Time.deltaTime);
			}

            Console.Instance.OutputToConsole(string.Format("{0}: Starting up {1} {2}",  this.ToString(), ApplicationTitle, ApplicationVersion), Console.Instance.Style_Admin);

            mGameState = GameStates.STATES.BOOTUP;

            BootupSequence();

         mFirstTimeLoaded = false;
        }
    }
}
