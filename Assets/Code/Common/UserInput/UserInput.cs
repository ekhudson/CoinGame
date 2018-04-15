using UnityEngine;
using System.Collections.Generic;
using XInputDotNetPure; // Required in C#
using Sirenix.OdinInspector;

public class UserInput : Singleton<UserInput>
{
    [SerializeField]
    private float m_MouseSensitivityVertical = 1f;
    [SerializeField]
    private float m_MouseSensitivityHorizontal = 1f;
    [SerializeField]
    private float m_JoystickDeadzoneX = 0.2f;
    [SerializeField]
    private float m_JoystickDeadzoneY = 0.2f;
    [SerializeField]
    [InlineEditor]
    private UserActionSetAsset m_ActionSetAsset;

    private Dictionary<string, List<UserAction>> mActionsDictionary = new Dictionary<string, List<UserAction>>();

    private Dictionary<KeyCode, List<UserAction>> mKeyBindingsDictionary = new Dictionary<KeyCode, List<UserAction>>();
    private Dictionary<KeyBinding.MouseButtons, List<UserAction>> mMouseBindingsDictionary = new Dictionary<KeyBinding.MouseButtons, List<UserAction>>();
	private Dictionary<KeyBinding.GamePadButtonValues, List<UserAction>> mGamepPadButtonBindings = new Dictionary<KeyBinding.GamePadButtonValues, List<UserAction>>();
	private Dictionary<KeyBinding.GamePadJoystickValues, List<UserAction>> mGamepadJoystickBindings = new Dictionary<KeyBinding.GamePadJoystickValues, List<UserAction>>();

    private List<UserAction> mKeysDown = new List<UserAction>();

	private Dictionary<int, List<UserAction>> mKeyDownDict = new Dictionary<int, List<UserAction>>();
	
	private List<int> mConnectControllerIndexes = new List<int>();

    public bool IsGamePadActive(int gamepadID)
    {
        PlayerIndex testPlayerIndex = (PlayerIndex)gamepadID;
        GamePadState testState = GamePad.GetState(testPlayerIndex);
        if (testState.IsConnected)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void Start ()
    {
        GatherKeyBindings();
        StoreKeyBindings();
        mKeysDown.Clear();
		GetConnectedControllers();

		for(int i = 0; i < 4; i++)
		{
			mKeyDownDict.Add(i, new List<UserAction>());
		}
    }

	private void GetConnectedControllers()
	{
		for (int i = 0; i < 4; ++i)
		{
			PlayerIndex testPlayerIndex = (PlayerIndex)i;
			GamePadState testState = GamePad.GetState(testPlayerIndex);
			if (testState.IsConnected)
			{
				Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
				mConnectControllerIndexes.Add(i);
			}
		}
	}

    public void GatherKeyBindings()
    {
        if (m_ActionSetAsset == null)
        {
            Debug.LogError("No Key Bindings Asset Specified! Input disabled.");
            enabled = false;
            return;
        }

        foreach(UserActionSet set in m_ActionSetAsset.ActionSets)
        {
            List<UserAction> actions = new List<UserAction>();

            foreach(UserActionDefinition definition in set.Actions)
            {
                actions.Add(new UserAction(definition.Name, definition.Binding));
            }

            mActionsDictionary.Add(set.SetName, actions);
        }
    }
    
    private void StoreKeyBindings()
    {

    }

    //Store all the KeyBindings for easy referencing
    private void ParseActionSet(List<UserAction> setList)
    {
        foreach(UserAction action in setList)
        {
            if (action.Binding.Key != KeyCode.None)
            {
                if (!mKeyBindingsDictionary.ContainsKey(action.Binding.Key))
                {
                    mKeyBindingsDictionary.Add(action.Binding.Key, new List<UserAction>(){ action } );
                }
                else
                {
                    mKeyBindingsDictionary[action.Binding.Key].Add(action);
                }
            }

            if (action.Binding.AltKey != KeyCode.None)
            {
                if (!mKeyBindingsDictionary.ContainsKey(action.Binding.AltKey))
                {
                    mKeyBindingsDictionary.Add(action.Binding.AltKey, new List<UserAction>(){ action });
                }
                else
                {
                    mKeyBindingsDictionary[action.Binding.AltKey].Add(action);
                }
            }

            if (action.Binding.MouseButton != KeyBinding.MouseButtons.None)
            {
                if (!mMouseBindingsDictionary.ContainsKey(action.Binding.MouseButton))
                {
                    mMouseBindingsDictionary.Add(action.Binding.MouseButton, new List<UserAction>(){ action });
                }
                else
                {
                    mMouseBindingsDictionary[action.Binding.MouseButton].Add(action);
                }
            }

            if (action.Binding.AltMouseButton != KeyBinding.MouseButtons.None)
            {
                if (!mMouseBindingsDictionary.ContainsKey(action.Binding.AltMouseButton))
                {
                    mMouseBindingsDictionary.Add(action.Binding.AltMouseButton, new List<UserAction>(){ action });
                }
                else
                {
                    mMouseBindingsDictionary[action.Binding.AltMouseButton].Add(action);
                }
            }

			if (action.Binding.ControllerButtons != KeyBinding.GamePadButtonValues.None)
			{
				if (!mGamepPadButtonBindings.ContainsKey(action.Binding.ControllerButtons))
				{
					mGamepPadButtonBindings.Add(action.Binding.ControllerButtons, new List<UserAction>(){ action });
				}
				else
				{
					mGamepPadButtonBindings[action.Binding.ControllerButtons].Add(action);
				}
			}

			if (action.Binding.ControllerJoysticks != KeyBinding.GamePadJoystickValues.None)
			{
				if (!mGamepadJoystickBindings.ContainsKey(action.Binding.ControllerJoysticks))
				{
					mGamepadJoystickBindings.Add(action.Binding.ControllerJoysticks, new List<UserAction>(){ action });
				}
				else
				{
					mGamepadJoystickBindings[action.Binding.ControllerJoysticks].Add(action);
				}
			}
        }
    }     

    private void Update ()
    {
        //foreach(UserAction binding in mKeysDown)
        //{
        //    EventManager.Instance.Post(new UserInputEvent(UserInputEvent.TYPE.KEYHELD, binding, 0, Vector3.zero, this));
        //}
    }

    private void OnGUI()
    {
        Event e = Event.current;

        if (e.isKey && e.keyCode != KeyCode.None)
        {
            if(e.type == EventType.KeyDown)
            {
                ProcessKeycode(e.keyCode, UserInputEvent.TYPE.KEYDOWN);
            }

            if(e.type == EventType.KeyUp)
            {
                ProcessKeycode(e.keyCode, UserInputEvent.TYPE.KEYUP);
            }
        }
        else if (e.isMouse && e.type == EventType.MouseDown || e.type == EventType.MouseUp)
        {
            ProcessMouseInput(e.button, e.type);
        }

		GatherGamePadInput();
		GatherJoystickInput();
    }

    private void ProcessKeycode(KeyCode code, UserInputEvent.TYPE inputType)
    {
        if (!mKeyBindingsDictionary.ContainsKey(code))
        {
            return;
        }

        foreach(UserAction action in mKeyBindingsDictionary[code])
        {
            if (action.Enabled)
            {
                if (inputType == UserInputEvent.TYPE.KEYDOWN && mKeysDown.Contains(action))
				{
					inputType = UserInputEvent.TYPE.KEYHELD;
				}

				//EventManager.Instance.Post(new UserInputEvent(inputType, action, 0, Vector3.zero, this)); //TODO: Figure out how to get proper player index

                if (inputType == UserInputEvent.TYPE.KEYDOWN)
                {
                    action.State = UserAction.ActionStates.PRESSED;

                    if (!mKeysDown.Contains(action))
                    {
                        mKeysDown.Add(action);
                    }
                }
                else if (inputType == UserInputEvent.TYPE.KEYUP)
                {
                    action.State = UserAction.ActionStates.NONE;

                    if (mKeysDown.Contains(action))
                    {
                        mKeysDown.Remove(action);
                    }
                }
            }
        }
    }

    private void ProcessMouseInput(int button, EventType evtType)
    {
        KeyBinding.MouseButtons mouseButton = (KeyBinding.MouseButtons)(button + 1);
        UserInputEvent.TYPE inputType = evtType == EventType.MouseDown ? UserInputEvent.TYPE.KEYDOWN : UserInputEvent.TYPE.KEYUP;

        if (!mMouseBindingsDictionary.ContainsKey(mouseButton))
        {
            return;
        }

        foreach(UserAction binding in mMouseBindingsDictionary[mouseButton])
        {
            if (binding.Enabled)
            {
               // EventManager.Instance.Post(new UserInputEvent(inputType, binding, 0, Vector3.zero, this));

                if (inputType == UserInputEvent.TYPE.KEYDOWN)
                {
                    binding.State = UserAction.ActionStates.PRESSED;

                    if (!mKeysDown.Contains(binding))
                    {
                        mKeysDown.Add(binding);
                    }
                }
                else if (inputType == UserInputEvent.TYPE.KEYUP)
                {
                    binding.State = UserAction.ActionStates.NONE;

                    if (mKeysDown.Contains(binding))
                    {
                        mKeysDown.Remove(binding);
                    }
                }
            }
        }
    }

	private void GatherGamePadInput()
	{
		for(int i = 0; i < mConnectControllerIndexes.Count; i++)
		{
			int controllerIndex = mConnectControllerIndexes[i];		
			PlayerIndex playerIndex = (PlayerIndex)controllerIndex;
			GamePadState state = new GamePadState();

			//Debug.Log ("Getting state for player " + playerIndex.ToString());

			state = GamePad.GetState(playerIndex);
			
			if (!state.IsConnected)
			{
				Debug.LogWarning(string.Format("Controller {0} has been disconnected!", controllerIndex.ToString()));
				mConnectControllerIndexes.Remove(controllerIndex);
				continue;
			}
			
			ProcessGamePadInput(state, playerIndex);
		}
	}

	private void ProcessGamePadInput(GamePadState state, PlayerIndex playerIndex)
	{
		//Debug.Log ("Processing for player " + playerIndex.ToString());
		ProcessGamePadButton(KeyBinding.GamePadButtonValues.A, state.Buttons.A, playerIndex);
		ProcessGamePadButton(KeyBinding.GamePadButtonValues.B, state.Buttons.B, playerIndex);
		ProcessGamePadButton(KeyBinding.GamePadButtonValues.X, state.Buttons.X, playerIndex);
		ProcessGamePadButton(KeyBinding.GamePadButtonValues.Y, state.Buttons.Y, playerIndex);
		ProcessGamePadButton(KeyBinding.GamePadButtonValues.LeftThumb, state.Buttons.LeftStick, playerIndex);
		ProcessGamePadButton(KeyBinding.GamePadButtonValues.RightThumb, state.Buttons.RightStick, playerIndex);
		ProcessGamePadButton(KeyBinding.GamePadButtonValues.LeftShoulder, state.Buttons.LeftShoulder, playerIndex);
		ProcessGamePadButton(KeyBinding.GamePadButtonValues.RightShoulder, state.Buttons.RightShoulder, playerIndex);
		ProcessGamePadButton(KeyBinding.GamePadButtonValues.Back, state.Buttons.Back, playerIndex);
		ProcessGamePadButton(KeyBinding.GamePadButtonValues.DPadUp, state.DPad.Up, playerIndex);
		ProcessGamePadButton(KeyBinding.GamePadButtonValues.DPadDown, state.DPad.Down, playerIndex);
		ProcessGamePadButton(KeyBinding.GamePadButtonValues.DPadLeft, state.DPad.Left, playerIndex);
		ProcessGamePadButton(KeyBinding.GamePadButtonValues.DPadRight, state.DPad.Right, playerIndex);
	}

	private void ProcessGamePadButton(KeyBinding.GamePadButtonValues button, ButtonState buttonState, PlayerIndex playerIndex)
	{
		if (!mGamepPadButtonBindings.ContainsKey(button))
		{
			return;
		}

		int playerIndexInt = (int)playerIndex;

		foreach(UserAction binding in mGamepPadButtonBindings[button])
		{
			if (buttonState == ButtonState.Released && !mKeyDownDict[playerIndexInt].Contains(binding))
			{
				continue;
			}

			if (binding.Enabled)
			{
				if (!mKeyDownDict[playerIndexInt].Contains(binding) && buttonState == ButtonState.Pressed)
				{
					mKeyDownDict[playerIndexInt].Add(binding);
					//EventManager.Instance.Post(new UserInputEvent(UserInputEvent.TYPE.GAMEPAD_BUTTON_DOWN, binding, playerIndexInt, Vector3.zero, this));
				}
				else if (mKeyDownDict[playerIndexInt].Contains(binding) && buttonState == ButtonState.Released)
				{
					mKeyDownDict[playerIndexInt].Remove(binding);
					//EventManager.Instance.Post(new UserInputEvent(UserInputEvent.TYPE.GAMEPAD_BUTTON_UP, binding, playerIndexInt, Vector3.zero, this));
				}
				else if (mKeyDownDict[playerIndexInt].Contains(binding) && buttonState == ButtonState.Pressed)
				{
					//EventManager.Instance.Post(new UserInputEvent(UserInputEvent.TYPE.GAMEPAD_BUTTON_HELD, binding, playerIndexInt, Vector3.zero, this));
				}
			}
		}
	}

	//THIS IS A COPY OF GATHER GAME PAD INPUT, DO I NEED BOTH???
	private void GatherJoystickInput()
	{
		for(int i = 0; i < mConnectControllerIndexes.Count; i++)
		{
			int controllerIndex = mConnectControllerIndexes[i];		
			PlayerIndex playerIndex = (PlayerIndex)controllerIndex;
			GamePadState state = GamePad.GetState(playerIndex);

			if (!state.IsConnected)
			{
				Debug.LogWarning(string.Format("Controller {0} has been disconnected!", controllerIndex.ToString()));
				mConnectControllerIndexes.Remove(controllerIndex);
				continue;
			}

			GatherJoystickInput(state, playerIndex);
		}
	}

	private void GatherJoystickInput(GamePadState state, PlayerIndex playerIndex)
	{
		if(state.Triggers.Left > 0)
		{
			ProcessJoystickInput(KeyBinding.GamePadJoystickValues.LeftTrigger, state.Triggers.Left, 0, playerIndex);
		}

		if(state.Triggers.Right > 0)
		{
			ProcessJoystickInput(KeyBinding.GamePadJoystickValues.RightTrigger, state.Triggers.Right, 0, playerIndex);
		}

		//if (state.ThumbSticks.Left.X > 0 || state.ThumbSticks.Left.Y > 0)
		//{
			ProcessJoystickInput(KeyBinding.GamePadJoystickValues.LeftStick, state.ThumbSticks.Left.X, state.ThumbSticks.Left.Y, playerIndex);
		//}

		//if (state.ThumbSticks.Right.X > 0 || state.ThumbSticks.Right.Y > 0)
		//{
			ProcessJoystickInput(KeyBinding.GamePadJoystickValues.RightStick, state.ThumbSticks.Right.X, state.ThumbSticks.Right.Y, playerIndex);
		//}
	}

	private void ProcessJoystickInput(KeyBinding.GamePadJoystickValues joystick, float valueX, float valueY, PlayerIndex playerIndex)
	{
		if (!mGamepadJoystickBindings.ContainsKey(joystick))
		{
			return;
		}

		if (Mathf.Abs(valueX) < m_JoystickDeadzoneX)
		{
			valueX = 0;
		}

		if (Mathf.Abs(valueY) < m_JoystickDeadzoneY)
		{
			valueY = 0;
		}

		foreach(UserAction binding in mGamepadJoystickBindings[joystick])
		{
			if (binding.Enabled)
			{
				//EventManager.Instance.Post(new UserInputEvent(UserInputEvent.TYPE.GAMEPAD_JOYSTICK, binding, new UserInputEvent.JoystickInfoClass(valueX, valueY), (int)playerIndex, Vector3.zero, this));
			}
		}
	}

    /// <summary>
    /// Enables or disables a binding.
    /// </summary>
    /// <param name='action'>
    /// Binding.
    /// </param>
    /// <param name='enable'>
    /// Enable (true) / Disable (false).
    /// </param>
    public void EnableBinding(string set, UserAction action, bool enable)
    {
        if(mActionsDictionary[set].Contains(action))
        {
                action.Enabled = enable;
        }
    }
}
