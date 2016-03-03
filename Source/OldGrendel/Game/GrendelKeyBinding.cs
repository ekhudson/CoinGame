using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using XInputDotNetPure;

[System.Serializable]
public class GrendelKeyBinding
{
    public string BindingName = "New Binding";
    public KeyCode Key = KeyCode.A;
    public KeyCode AltKey = KeyCode.B;
    public bool Enabled = true;
    public MouseButtons MouseButton = MouseButtons.None;
    public MouseButtons AltMouseButton = MouseButtons.None;
	public GamePadButtonValues ControllerButtons;
	public GamePadJoystickValues ControllerJoysticks;

	public List<GrendelKeyBinding> Conflicts = new List<GrendelKeyBinding>(); //TODO: Figure out the most efficient way to update keybind conflicts

	[System.NonSerialized]
	private bool mIsDown = false;

	public GrendelKeyBinding(string bindingName, KeyCode key, KeyCode altKey, MouseButtons mouseButton, MouseButtons altMouseButton, GamePadButtonValues controllerButtons, GamePadJoystickValues joysticks)
	{
		BindingName = bindingName;
		Key = key;
		AltKey = altKey;
		MouseButton = mouseButton;
		AltMouseButton = altMouseButton;
		ControllerButtons = controllerButtons;
		ControllerJoysticks = joysticks;
	}

	public GrendelKeyBinding(string bindingName, KeyCode key, KeyCode altKey, MouseButtons mouseButton, MouseButtons altMouseButton, GamePadButtonValues controllerButtons)
	{
		BindingName = bindingName;
		Key = key;
		AltKey = altKey;
		MouseButton = mouseButton;
		AltMouseButton = altMouseButton;
		ControllerButtons = controllerButtons;
	}

    public GrendelKeyBinding(string bindingName, KeyCode key, KeyCode altKey, MouseButtons mouseButton, MouseButtons altMouseButton)
    {
        BindingName = bindingName;
        Key = key;
        AltKey = altKey;
        MouseButton = mouseButton;
        AltMouseButton = altMouseButton;
    }

    public GrendelKeyBinding(string bindingName, KeyCode key, KeyCode altKey)
    {
        BindingName = bindingName;
        Key = key;
        AltKey = altKey;
        MouseButton = MouseButtons.None;
        AltMouseButton = MouseButtons.None;
    }

    public bool IsDown
    {
        get
        {
            return mIsDown;
        }
        set
        {
            mIsDown = value;
        }
    }

    public enum MouseButtons
    {
        None = 0,
        One = 1,
        Two,
        Three,
        Four,
        Five,
        Six,
    }

	public enum GamePadButtonValues
	{
		None = 0,
		DPadUp = 1,
		DPadDown = 2,
		DPadLeft = 4,
		DPadRight = 8,
		Start = 16,
		Back = 32,
		LeftThumb = 64,
		RightThumb = 128,
		LeftShoulder = 256,
		RightShoulder = 512,
		A = 4096,
		B = 8192,
		X = 16384,
		Y = 32768,
	}

	public enum GamePadJoystickValues
	{
		None = 0, 
		LeftTrigger = 1,
		RightTrigger = 2,
		LeftStick = 4,
		RightStick = 8,
	}


}
