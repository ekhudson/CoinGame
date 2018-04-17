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
    private KeyBindingsAsset m_BindingsAsset;

    private List<KeyBinding> mKeyBindings = new List<KeyBinding>();

    private Dictionary<KeyCode, List<KeyBinding>> mKeyBindingsDictionary = new Dictionary<KeyCode, List<KeyBinding>>();
    private Dictionary<KeyBinding.MouseButtons, List<KeyBinding>> mMouseButtonBindingsDictionary = new Dictionary<KeyBinding.MouseButtons, List<KeyBinding>>();
    private Dictionary<KeyBinding.MouseAxes, List<KeyBinding>> mMouseAxesBindingsDictionary = new Dictionary<KeyBinding.MouseAxes, List<KeyBinding>>();
    private Dictionary<KeyBinding.GamePadButtonValues, List<KeyBinding>> mGamepPadButtonBindings = new Dictionary<KeyBinding.GamePadButtonValues, List<KeyBinding>>();
	private Dictionary<KeyBinding.GamePadJoystickValues, List<KeyBinding>> mGamepadJoystickBindings = new Dictionary<KeyBinding.GamePadJoystickValues, List<KeyBinding>>();

    private List<KeyBinding> mKeysDown = new List<KeyBinding>();

	private Dictionary<int, List<KeyBinding>> mKeyDownDict = new Dictionary<int, List<KeyBinding>>();
	
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
			mKeyDownDict.Add(i, new List<KeyBinding>());
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
        if (m_BindingsAsset == null)
        {
            Debug.LogError("No Key Bindings Asset Specified! Input disabled.");
            enabled = false;
            return;
        }

        mKeyBindings = new List<KeyBinding>(m_BindingsAsset.KeyBindings);
    }

    //Store all the KeyBindings for easy referencing
    private void StoreKeyBindings()
    {
        foreach(KeyBinding binding in mKeyBindings)
        {
            if (binding.Key != KeyCode.None)
            {
                if (!mKeyBindingsDictionary.ContainsKey(binding.Key))
                {
                    mKeyBindingsDictionary.Add(binding.Key, new List<KeyBinding>(){ binding } );
                }
                else
                {
                    mKeyBindingsDictionary[binding.Key].Add(binding);
                }
            }

            if (binding.AltKey != KeyCode.None)
            {
                if (!mKeyBindingsDictionary.ContainsKey(binding.AltKey))
                {
                    mKeyBindingsDictionary.Add(binding.AltKey, new List<KeyBinding>(){ binding });
                }
                else
                {
                    mKeyBindingsDictionary[binding.AltKey].Add(binding);
                }
            }

            if (binding.MouseButton != KeyBinding.MouseButtons.None)
            {
                if (!mMouseButtonBindingsDictionary.ContainsKey(binding.MouseButton))
                {
                    mMouseButtonBindingsDictionary.Add(binding.MouseButton, new List<KeyBinding>(){ binding });
                }
                else
                {
                    mMouseButtonBindingsDictionary[binding.MouseButton].Add(binding);
                }
            }

            if (binding.AltMouseButton != KeyBinding.MouseButtons.None)
            {
                if (!mMouseButtonBindingsDictionary.ContainsKey(binding.AltMouseButton))
                {
                    mMouseButtonBindingsDictionary.Add(binding.AltMouseButton, new List<KeyBinding>(){ binding });
                }
                else
                {
                    mMouseButtonBindingsDictionary[binding.AltMouseButton].Add(binding);
                }
            }

			if (binding.ControllerButtons != KeyBinding.GamePadButtonValues.None)
			{
				if (!mGamepPadButtonBindings.ContainsKey(binding.ControllerButtons))
				{
					mGamepPadButtonBindings.Add(binding.ControllerButtons, new List<KeyBinding>(){ binding });
				}
				else
				{
					mGamepPadButtonBindings[binding.ControllerButtons].Add(binding);
				}
			}

			if (binding.ControllerJoysticks != KeyBinding.GamePadJoystickValues.None)
			{
				if (!mGamepadJoystickBindings.ContainsKey(binding.ControllerJoysticks))
				{
					mGamepadJoystickBindings.Add(binding.ControllerJoysticks, new List<KeyBinding>(){ binding });
				}
				else
				{
					mGamepadJoystickBindings[binding.ControllerJoysticks].Add(binding);
				}
			}
        }
    }     

    private void Update ()
    {
        foreach(KeyBinding binding in mKeysDown)
        {
            EventManager.Instance.Post(new UserInputEvent(UserInputEvent.TYPE.KEYHELD, binding, 0, Vector3.zero, this));
        }
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
            ProcessMouseButtonInput(e.button, e.type);
        }
        else if (e.isMouse && e.type == EventType.MouseMove)
        {
            EvaluateMouseAxesInput(e.delta);
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

        foreach(KeyBinding binding in mKeyBindingsDictionary[code])
        {
            if (binding.Enabled)
            {
                if (inputType == UserInputEvent.TYPE.KEYDOWN && mKeysDown.Contains(binding))
				{
					inputType = UserInputEvent.TYPE.KEYHELD;
				}

				EventManager.Instance.Post(new UserInputEvent(inputType, binding, 0, Vector3.zero, this)); //TODO: Figure out how to get proper player index

                if (inputType == UserInputEvent.TYPE.KEYDOWN)
                {
                    binding.IsDown = true;

                    if (!mKeysDown.Contains(binding))
                    {
                        mKeysDown.Add(binding);
                    }
                }
                else if (inputType == UserInputEvent.TYPE.KEYUP)
                {
                    binding.IsDown = false;

                    if (mKeysDown.Contains(binding))
                    {
                        mKeysDown.Remove(binding);
                    }
                }
            }
        }
    }

    private void EvaluateMouseAxesInput(Vector2 mouseDelta)
    {
        float x = 0f;
        float y = 0f;

        foreach(KeyValuePair<KeyBinding.MouseAxes, List<KeyBinding>> pair in mMouseAxesBindingsDictionary)
        {
            if (pair.Key == KeyBinding.MouseAxes.X && mouseDelta.x != 0)
            {
                x = mouseDelta.x;
                ProcessMouseAxesInput(pair.Value, x, y);
            }
            else if (pair.Key == KeyBinding.MouseAxes.Y && mouseDelta.y != 0)
            {
                y = mouseDelta.y;
                ProcessMouseAxesInput(pair.Value, x, y);
            }
            else if (pair.Key == KeyBinding.MouseAxes.Both && (mouseDelta.x != 0 || mouseDelta.y != 0))
            {
                x = mouseDelta.x;
                y = mouseDelta.y;
                ProcessMouseAxesInput(pair.Value, x, y);
            }
            else
            {
                foreach (KeyBinding binding in pair.Value)
                {
                    if (binding.IsDown)
                    {
                        binding.IsDown = false;
                        mKeysDown.Remove(binding);
                    }
                }
            }
        }
    }

    private void ProcessMouseAxesInput(List<KeyBinding> bindings, float x, float y)
    {
        foreach(KeyBinding binding in bindings)
        {
            if (!binding.IsDown)
            {
                mKeysDown.Add(binding);
                binding.IsDown = true;
            }

            EventManager.Instance.Post(new UserInputEvent(UserInputEvent.TYPE.MOUSE_MOVE, binding, new UserInputEvent.MouseDeltaInfoClass(x, y), 0, Vector3.zero, this));
        }
    }

    private void ProcessMouseButtonInput(int button, EventType evtType)
    {
        KeyBinding.MouseButtons mouseButton = (KeyBinding.MouseButtons)(button + 1);
        UserInputEvent.TYPE inputType = evtType == EventType.MouseDown ? UserInputEvent.TYPE.KEYDOWN : UserInputEvent.TYPE.KEYUP;

        if (!mMouseButtonBindingsDictionary.ContainsKey(mouseButton))
        {
            return;
        }

        foreach(KeyBinding binding in mMouseButtonBindingsDictionary[mouseButton])
        {
            if (binding.Enabled)
            {
                EventManager.Instance.Post(new UserInputEvent(inputType, binding, 0, Vector3.zero, this));

                if (inputType == UserInputEvent.TYPE.KEYDOWN)
                {
                    binding.IsDown = true;

                    if (!mKeysDown.Contains(binding))
                    {
                        mKeysDown.Add(binding);
                    }
                }
                else if (inputType == UserInputEvent.TYPE.KEYUP)
                {
                    binding.IsDown = false;

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

		foreach(KeyBinding binding in mGamepPadButtonBindings[button])
		{
			if (buttonState == ButtonState.Released && !mKeyDownDict[playerIndexInt].Contains(binding))
			{
				continue;
			}

			if (binding.Enabled)
			{
				if (!mKeyDownDict[playerIndexInt].Contains(binding) && buttonState == ButtonState.Pressed)
				{
                    binding.IsDown = true;
					mKeyDownDict[playerIndexInt].Add(binding);
					EventManager.Instance.Post(new UserInputEvent(UserInputEvent.TYPE.GAMEPAD_BUTTON_DOWN, binding, playerIndexInt, Vector3.zero, this));
				}
				else if (mKeyDownDict[playerIndexInt].Contains(binding) && buttonState == ButtonState.Released)
				{
                    binding.IsDown = false;
                    mKeyDownDict[playerIndexInt].Remove(binding);
					EventManager.Instance.Post(new UserInputEvent(UserInputEvent.TYPE.GAMEPAD_BUTTON_UP, binding, playerIndexInt, Vector3.zero, this));
				}
				else if (mKeyDownDict[playerIndexInt].Contains(binding) && buttonState == ButtonState.Pressed)
				{
                    binding.IsDown = true;
                    EventManager.Instance.Post(new UserInputEvent(UserInputEvent.TYPE.GAMEPAD_BUTTON_HELD, binding, playerIndexInt, Vector3.zero, this));
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
		//if(state.Triggers.Left > 0)
		//{
			ProcessJoystickInput(KeyBinding.GamePadJoystickValues.LeftTrigger, state.Triggers.Left, 0, playerIndex);
		//}

		//if(state.Triggers.Right > 0)
		//{
			ProcessJoystickInput(KeyBinding.GamePadJoystickValues.RightTrigger, state.Triggers.Right, 0, playerIndex);
		//}

		//if (state.ThumbSticks.Left.X != 0 || state.ThumbSticks.Left.Y != 0)
		//{
			ProcessJoystickInput(KeyBinding.GamePadJoystickValues.LeftStick, state.ThumbSticks.Left.X, state.ThumbSticks.Left.Y, playerIndex);
		//}

		//if (state.ThumbSticks.Right.X != 0 || state.ThumbSticks.Right.Y != 0)
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

        bool isDown = valueY != 0 || valueX != 0;


        foreach (KeyBinding binding in mGamepadJoystickBindings[joystick])
		{            
            if (!isDown && !binding.IsDown)
            {
                return;
            }

            binding.IsDown = isDown;

			if (binding.Enabled)
			{
				EventManager.Instance.Post(new UserInputEvent(UserInputEvent.TYPE.GAMEPAD_JOYSTICK, binding, new UserInputEvent.JoystickInfoClass(valueX, valueY), (int)playerIndex, Vector3.zero, this));
			}
		}
	}

    /// <summary>
    /// Enables or disables a binding.
    /// </summary>
    /// <param name='binding'>
    /// Binding.
    /// </param>
    /// <param name='enable'>
    /// Enable (true) / Disable (false).
    /// </param>
    public void EnableBinding(KeyBinding binding, bool enable)
    {
        if(mKeyBindings.Contains(binding))
        {
                binding.Enabled = enable;
        }
    }

    /// <summary>
    /// Enables or disables several bindings.
    /// </summary>
    /// <param name='bindings'>
    /// Array of bindings.
    /// </param>
    /// <param name='enable'>
    /// Enable (true) / Disable (false).
    /// </param>
    public void EnableBindings(KeyBinding[] bindings, bool enable)
    {
        foreach(KeyBinding binding in bindings)
        {
            EnableBinding(binding, enable);
        }
    }
}
