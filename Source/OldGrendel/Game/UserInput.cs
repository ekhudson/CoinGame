using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using XInputDotNetPure; // Required in C#

public class UserInput<T> : Singleton<T> where T  : MonoBehaviour
{
    public float MouseSensitivityVertical = 1f;
    public float MouseSensitivityHorizontal = 1f;

	public float JoystickDeadzoneX = 0.2f;
	public float JoystickDeadzoneY = 0.2f;

	[HideInInspector]public GrendelKeyBinding MoveUp = new GrendelKeyBinding("Move Up", KeyCode.W, KeyCode.UpArrow, GrendelKeyBinding.MouseButtons.None, GrendelKeyBinding.MouseButtons.None);
	[HideInInspector]public GrendelKeyBinding MoveDown = new GrendelKeyBinding("Move Down", KeyCode.S, KeyCode.DownArrow, GrendelKeyBinding.MouseButtons.None, GrendelKeyBinding.MouseButtons.None);
    [HideInInspector]public GrendelKeyBinding MoveLeft = new GrendelKeyBinding("Move Left", KeyCode.A, KeyCode.LeftArrow, GrendelKeyBinding.MouseButtons.None, GrendelKeyBinding.MouseButtons.None);
	[HideInInspector]public GrendelKeyBinding MoveRight = new GrendelKeyBinding("Move Right", KeyCode.D, KeyCode.RightArrow, GrendelKeyBinding.MouseButtons.None, GrendelKeyBinding.MouseButtons.None);
	[HideInInspector]public GrendelKeyBinding Run = new GrendelKeyBinding("Run", KeyCode.LeftShift, KeyCode.RightShift, GrendelKeyBinding.MouseButtons.None, GrendelKeyBinding.MouseButtons.None);
	[HideInInspector]public GrendelKeyBinding PrimaryFire = new GrendelKeyBinding("Primary Fire", KeyCode.None, KeyCode.None, GrendelKeyBinding.MouseButtons.One, GrendelKeyBinding.MouseButtons.None);
	[HideInInspector]public GrendelKeyBinding SecondaryFire = new GrendelKeyBinding("Secondary Fire", KeyCode.None, KeyCode.None, GrendelKeyBinding.MouseButtons.Two, GrendelKeyBinding.MouseButtons.None);


    [HideInInspector]public List<GrendelKeyBinding> KeyBindings = new List<GrendelKeyBinding>();

	[System.NonSerialized]
    private Dictionary<KeyCode, List<GrendelKeyBinding>> mGrendelKeyBindingsDictionary = new Dictionary<KeyCode, List<GrendelKeyBinding>>();
    
	[System.NonSerialized]
	private Dictionary<GrendelKeyBinding.MouseButtons, List<GrendelKeyBinding>> mMouseBindingsDictionary = new Dictionary<GrendelKeyBinding.MouseButtons, List<GrendelKeyBinding>>();

	[System.NonSerialized]
	private Dictionary<GrendelKeyBinding.GamePadButtonValues, List<GrendelKeyBinding>> mGamepPadButtonBindings = new Dictionary<GrendelKeyBinding.GamePadButtonValues, List<GrendelKeyBinding>>();

	[System.NonSerialized]
	private Dictionary<GrendelKeyBinding.GamePadJoystickValues, List<GrendelKeyBinding>> mGamepadJoystickBindings = new Dictionary<GrendelKeyBinding.GamePadJoystickValues, List<GrendelKeyBinding>>();

	[System.NonSerialized]
    private List<GrendelKeyBinding> mKeysDown = new List<GrendelKeyBinding>();

	[System.NonSerialized]
	private Dictionary<int, List<GrendelKeyBinding>> mKeyDownDict = new Dictionary<int, List<GrendelKeyBinding>>();
	
	[System.NonSerialized]
	private List<int> mConnectControllerIndexes = new List<int>();
    
    // Use this for initialization
    private void Start ()
    {
        GatherKeyBindings(this.GetType());
        StoreGrendelKeyBindings();
        mKeysDown.Clear();
		GetConnectedControllers();

		for(int i = 0; i < 4; i++)
		{
			mKeyDownDict.Add(i, new List<GrendelKeyBinding>());
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

    //Find all the GrendelKeyBindings on UserInput
    public void GatherKeyBindings(System.Type t)
    {
        KeyBindings.Clear();

        System.Type myType = t;

        System.Reflection.FieldInfo[] myField = myType.GetFields();

        for(int i = 0; i < myField.Length; i++)
        {
            if(myField[i].FieldType == typeof(GrendelKeyBinding))
            {
                GrendelKeyBinding binding = (GrendelKeyBinding)myField[i].GetValue(this);
                if (!KeyBindings.Contains(binding))
                {
                    KeyBindings.Add(binding);
                }
            }
        }
    }

    //Store all the KeyBindings for easy referencing
    private void StoreGrendelKeyBindings()
    {
        foreach(GrendelKeyBinding binding in KeyBindings)
        {
            if (binding.Key != KeyCode.None)
            {
                if (!mGrendelKeyBindingsDictionary.ContainsKey(binding.Key))
                {
                    mGrendelKeyBindingsDictionary.Add(binding.Key, new List<GrendelKeyBinding>(){ binding } );
                }
                else
                {
                    mGrendelKeyBindingsDictionary[binding.Key].Add(binding);
                }
            }

            if (binding.AltKey != KeyCode.None)
            {
                if (!mGrendelKeyBindingsDictionary.ContainsKey(binding.AltKey))
                {
                    mGrendelKeyBindingsDictionary.Add(binding.AltKey, new List<GrendelKeyBinding>(){ binding });
                }
                else
                {
                    mGrendelKeyBindingsDictionary[binding.AltKey].Add(binding);
                }
            }

            if (binding.MouseButton != GrendelKeyBinding.MouseButtons.None)
            {
                if (!mMouseBindingsDictionary.ContainsKey(binding.MouseButton))
                {
                    mMouseBindingsDictionary.Add(binding.MouseButton, new List<GrendelKeyBinding>(){ binding });
                }
                else
                {
                    mMouseBindingsDictionary[binding.MouseButton].Add(binding);
                }
            }

            if (binding.AltMouseButton != GrendelKeyBinding.MouseButtons.None)
            {
                if (!mMouseBindingsDictionary.ContainsKey(binding.AltMouseButton))
                {
                    mMouseBindingsDictionary.Add(binding.AltMouseButton, new List<GrendelKeyBinding>(){ binding });
                }
                else
                {
                    mMouseBindingsDictionary[binding.AltMouseButton].Add(binding);
                }
            }

			if (binding.ControllerButtons != GrendelKeyBinding.GamePadButtonValues.None)
			{
				if (!mGamepPadButtonBindings.ContainsKey(binding.ControllerButtons))
				{
					mGamepPadButtonBindings.Add(binding.ControllerButtons, new List<GrendelKeyBinding>(){ binding });
				}
				else
				{
					mGamepPadButtonBindings[binding.ControllerButtons].Add(binding);
				}
			}

			if (binding.ControllerJoysticks != GrendelKeyBinding.GamePadJoystickValues.None)
			{
				if (!mGamepadJoystickBindings.ContainsKey(binding.ControllerJoysticks))
				{
					mGamepadJoystickBindings.Add(binding.ControllerJoysticks, new List<GrendelKeyBinding>(){ binding });
				}
				else
				{
					mGamepadJoystickBindings[binding.ControllerJoysticks].Add(binding);
				}
			}
        }
    }
     
    // Update is called once per frame
    private void Update ()
    { 
        if(Input.GetKeyDown(KeyCode.BackQuote))
        {
            if(GameOptions.Instance.DebugMode){ Console.Instance.ToggleConsole(); }
        }

        foreach(GrendelKeyBinding binding in mKeysDown)
        {
            EventManager.Instance.Post(new UserInputKeyEvent(UserInputKeyEvent.TYPE.KEYHELD, binding, 0, Vector3.zero, this));
        }
    }

    private void OnGUI()
    {
        Event e = Event.current;

        if (e.isKey && e.keyCode != KeyCode.None)
        {
            if(e.type == EventType.KeyDown)
            {
                ProcessKeycode(e.keyCode, UserInputKeyEvent.TYPE.KEYDOWN);
            }

            if(e.type == EventType.KeyUp)
            {
                ProcessKeycode(e.keyCode, UserInputKeyEvent.TYPE.KEYUP);
            }
        }
        else if (e.isMouse && e.type == EventType.mouseDown || e.type == EventType.mouseUp)
        {
            ProcessMouseInput(e.button, e.type);
        }

		GatherGamePadInput();
		GatherJoystickInput();
    }

    private void ProcessKeycode(KeyCode code, UserInputKeyEvent.TYPE inputType)
    {
        if (!mGrendelKeyBindingsDictionary.ContainsKey(code))
        {
            return;
        }

        foreach(GrendelKeyBinding binding in mGrendelKeyBindingsDictionary[code])
        {
            if (binding.Enabled)
            {
                if (inputType == UserInputKeyEvent.TYPE.KEYDOWN && mKeysDown.Contains(binding))
				{
					inputType = UserInputKeyEvent.TYPE.KEYHELD;
				}

				EventManager.Instance.Post(new UserInputKeyEvent(inputType, binding, 0, Vector3.zero, this)); //TODO: Figure out how to get proper player index

                if (inputType == UserInputKeyEvent.TYPE.KEYDOWN)
                {
                    binding.IsDown = true;

                    if (!mKeysDown.Contains(binding))
                    {
                        mKeysDown.Add(binding);
                    }
                }
                else if (inputType == UserInputKeyEvent.TYPE.KEYUP)
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

    private void ProcessMouseInput(int button, EventType evtType)
    {
        GrendelKeyBinding.MouseButtons mouseButton = (GrendelKeyBinding.MouseButtons)(button + 1);
        UserInputKeyEvent.TYPE inputType = evtType == EventType.MouseDown ? UserInputKeyEvent.TYPE.KEYDOWN : UserInputKeyEvent.TYPE.KEYUP;

        if (!mMouseBindingsDictionary.ContainsKey(mouseButton))
        {
            return;
        }

        foreach(GrendelKeyBinding binding in mMouseBindingsDictionary[mouseButton])
        {
            if (binding.Enabled)
            {
                EventManager.Instance.Post(new UserInputKeyEvent(inputType, binding, 0, Vector3.zero, this));

                if (inputType == UserInputKeyEvent.TYPE.KEYDOWN)
                {
                    binding.IsDown = true;

                    if (!mKeysDown.Contains(binding))
                    {
                        mKeysDown.Add(binding);
                    }
                }
                else if (inputType == UserInputKeyEvent.TYPE.KEYUP)
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
				Console.Instance.OutputToConsole(string.Format("Controller {0} has been disconnected!", controllerIndex.ToString()), Console.Instance.Style_Error);
				mConnectControllerIndexes.Remove(controllerIndex);
				continue;
			}
			
			ProcessGamePadInput(state, playerIndex);
		}
	}

	private void ProcessGamePadInput(GamePadState state, PlayerIndex playerIndex)
	{
		//Debug.Log ("Processing for player " + playerIndex.ToString());
		ProcessGamePadButton(GrendelKeyBinding.GamePadButtonValues.A, state.Buttons.A, playerIndex);
		ProcessGamePadButton(GrendelKeyBinding.GamePadButtonValues.B, state.Buttons.B, playerIndex);
		ProcessGamePadButton(GrendelKeyBinding.GamePadButtonValues.X, state.Buttons.X, playerIndex);
		ProcessGamePadButton(GrendelKeyBinding.GamePadButtonValues.Y, state.Buttons.Y, playerIndex);
		ProcessGamePadButton(GrendelKeyBinding.GamePadButtonValues.LeftThumb, state.Buttons.LeftStick, playerIndex);
		ProcessGamePadButton(GrendelKeyBinding.GamePadButtonValues.RightThumb, state.Buttons.RightStick, playerIndex);
		ProcessGamePadButton(GrendelKeyBinding.GamePadButtonValues.LeftShoulder, state.Buttons.LeftShoulder, playerIndex);
		ProcessGamePadButton(GrendelKeyBinding.GamePadButtonValues.RightShoulder, state.Buttons.RightShoulder, playerIndex);
		ProcessGamePadButton(GrendelKeyBinding.GamePadButtonValues.Back, state.Buttons.Back, playerIndex);
		ProcessGamePadButton(GrendelKeyBinding.GamePadButtonValues.DPadUp, state.DPad.Up, playerIndex);
		ProcessGamePadButton(GrendelKeyBinding.GamePadButtonValues.DPadDown, state.DPad.Down, playerIndex);
		ProcessGamePadButton(GrendelKeyBinding.GamePadButtonValues.DPadLeft, state.DPad.Left, playerIndex);
		ProcessGamePadButton(GrendelKeyBinding.GamePadButtonValues.DPadRight, state.DPad.Right, playerIndex);
	}

	private void ProcessGamePadButton(GrendelKeyBinding.GamePadButtonValues button, ButtonState buttonState, PlayerIndex playerIndex)
	{
		if (!mGamepPadButtonBindings.ContainsKey(button))
		{
			return;
		}

		int playerIndexInt = (int)playerIndex;

		foreach(GrendelKeyBinding binding in mGamepPadButtonBindings[button])
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
					EventManager.Instance.Post(new UserInputKeyEvent(UserInputKeyEvent.TYPE.GAMEPAD_BUTTON_DOWN, binding, playerIndexInt, Vector3.zero, this));
				}
				else if (mKeyDownDict[playerIndexInt].Contains(binding) && buttonState == ButtonState.Released)
				{
					mKeyDownDict[playerIndexInt].Remove(binding);
					EventManager.Instance.Post(new UserInputKeyEvent(UserInputKeyEvent.TYPE.GAMEPAD_BUTTON_UP, binding, playerIndexInt, Vector3.zero, this));
				}
				else if (mKeyDownDict[playerIndexInt].Contains(binding) && buttonState == ButtonState.Pressed)
				{
					EventManager.Instance.Post(new UserInputKeyEvent(UserInputKeyEvent.TYPE.GAMEPAD_BUTTON_HELD, binding, playerIndexInt, Vector3.zero, this));
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
				Console.Instance.OutputToConsole(string.Format("Controller {0} has been disconnected!", controllerIndex.ToString()), Console.Instance.Style_Error);
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
			ProcessJoystickInput(GrendelKeyBinding.GamePadJoystickValues.LeftTrigger, state.Triggers.Left, 0, playerIndex);
		}

		if(state.Triggers.Right > 0)
		{
			ProcessJoystickInput(GrendelKeyBinding.GamePadJoystickValues.RightTrigger, state.Triggers.Right, 0, playerIndex);
		}

		//if (state.ThumbSticks.Left.X > 0 || state.ThumbSticks.Left.Y > 0)
		//{
			ProcessJoystickInput(GrendelKeyBinding.GamePadJoystickValues.LeftStick, state.ThumbSticks.Left.X, state.ThumbSticks.Left.Y, playerIndex);
		//}

		//if (state.ThumbSticks.Right.X > 0 || state.ThumbSticks.Right.Y > 0)
		//{
			ProcessJoystickInput(GrendelKeyBinding.GamePadJoystickValues.RightStick, state.ThumbSticks.Right.X, state.ThumbSticks.Right.Y, playerIndex);
		//}
	}

	private void ProcessJoystickInput(GrendelKeyBinding.GamePadJoystickValues joystick, float valueX, float valueY, PlayerIndex playerIndex)
	{
		if (!mGamepadJoystickBindings.ContainsKey(joystick))
		{
			return;
		}

		if (Mathf.Abs(valueX) < JoystickDeadzoneX)
		{
			valueX = 0;
		}

		if (Mathf.Abs(valueY) < JoystickDeadzoneY)
		{
			valueY = 0;
		}

		foreach(GrendelKeyBinding binding in mGamepadJoystickBindings[joystick])
		{
			if (binding.Enabled)
			{
				EventManager.Instance.Post(new UserInputKeyEvent(UserInputKeyEvent.TYPE.GAMEPAD_JOYSTICK, binding, new UserInputKeyEvent.JoystickInfoClass(valueX, valueY), (int)playerIndex, Vector3.zero, this));
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
    public void EnableBinding(GrendelKeyBinding binding, bool enable)
    {
        if(KeyBindings.Contains(binding))
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
    public void EnableBindings(GrendelKeyBinding[] bindings, bool enable)
    {
        foreach(GrendelKeyBinding binding in bindings)
        {
            EnableBinding(binding, enable);
        }
    }
}
