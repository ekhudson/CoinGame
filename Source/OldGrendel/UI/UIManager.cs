using UnityEngine;
using System.Collections;

public class UIManager : Singleton<UIManager>
{
    [HideInInspector]public bool OverrideGamestate = false;

    [HideInInspector]public GameManager.GameStates.STATES OverrideState = GameManager.GameStates.STATES.MAINMENU;

    private const int kPanelWidth = 256;

    protected override void Awake()
    {
        base.Awake();
    }

	// Use this for initialization
	private void Start ()
    {
	
	}
	
	// Update is called once per frame
	private void OnGUI()
    {
        GameManager.GameStates.STATES state = OverrideGamestate == true ? OverrideState : GameManager.Instance.GameState;

        switch(state)
        {
            case GameManager.GameStates.STATES.LOADING:

                DrawLoader();

            break;

            case GameManager.GameStates.STATES.MAINMENU:

                DrawMainMenu();

            break;

            case GameManager.GameStates.STATES.RUNNING:

            break;

            case GameManager.GameStates.STATES.PAUSED:

            break;

            case GameManager.GameStates.STATES.OPTIONS:

                DrawOptions();

            break;

            case GameManager.GameStates.STATES.INTRO:

            break;

            case GameManager.GameStates.STATES.CREDITS:

            break;

            default:
            break;
        }
	}

    private void DrawLoader()
    {
        GUI.Label(new Rect(0,0, 100, 100), "Loading", GUI.skin.button);
    }

    private void DrawMainMenu()
    {
        GUILayout.BeginArea(new Rect(0,0, Screen.width, Screen.height));

        GUILayout.BeginHorizontal();

        GUILayout.FlexibleSpace();

            if (GUILayout.Button("Options", GUILayout.Width(kPanelWidth)))
            {
                GameManager.Instance.SetGameState(GameManager.GameStates.STATES.OPTIONS);
            }

        GUILayout.FlexibleSpace();

        GUILayout.EndHorizontal();

        GUILayout.EndArea();
    }

    private void DrawOptions()
    {
        GUILayout.BeginArea(new Rect(0,0, Screen.width, Screen.height));

        GUILayout.BeginHorizontal();

        GUILayout.FlexibleSpace();

            GUILayout.BeginVertical();

            GUILayout.FlexibleSpace();

                GUILayout.Label("Options", GUI.skin.box, GUILayout.Width(kPanelWidth));

                GUILayout.Label("Music Vol: ");
               // AudioManager.Instance.GlobalVolumeMusic = GUILayout.HorizontalSlider(AudioManager.Instance.GlobalVolumeMusic, 0f, 1f);

                GUILayout.Label("SFX Vol: ");
               // AudioManager.Instance.GlobalVolumeSFX = GUILayout.HorizontalSlider(AudioManager.Instance.GlobalVolumeSFX, 0f, 1f);

                if (GUI.changed)
                {
                   // AudioManager.Instance.UpdateAudio();
//                    PlayerPreferences.MusicVolume = AudioManager.Instance.GlobalVolumeMusic;
                   // PlayerPreferences.SFXVolume = AudioManager.Instance.GlobalVolumeSFX;
                    GUI.changed = false;
                }

                if (GUILayout.Button("Accept", GUILayout.Width(kPanelWidth)))
                {
                    GameManager.Instance.SetGameState(GameManager.GameStates.STATES.MAINMENU);
                }

            GUILayout.FlexibleSpace();

            GUILayout.EndVertical();

        GUILayout.FlexibleSpace();

        GUILayout.EndHorizontal();

        GUILayout.EndArea();
    }
}
