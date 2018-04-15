using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using XInputDotNetPure;

[System.Serializable]
public class KeyBinding
{
    public KeyCode Key = KeyCode.A;
    public KeyCode AltKey = KeyCode.B;
    public MouseButtons MouseButton = MouseButtons.None;
    public MouseButtons AltMouseButton = MouseButtons.None;
    public GamePadButtonValues ControllerButtons = GamePadButtonValues.None;
    public GamePadJoystickValues ControllerJoysticks = GamePadJoystickValues.None;

	public KeyBinding(KeyCode key, KeyCode altKey, MouseButtons mouseButton, MouseButtons altMouseButton, GamePadButtonValues controllerButtons, GamePadJoystickValues joysticks)
	{
		Key = key;
		AltKey = altKey;
		MouseButton = mouseButton;
		AltMouseButton = altMouseButton;
		ControllerButtons = controllerButtons;
		ControllerJoysticks = joysticks;
	}

	public KeyBinding(KeyCode key, KeyCode altKey, MouseButtons mouseButton, MouseButtons altMouseButton, GamePadButtonValues controllerButtons)
	{
		Key = key;
		AltKey = altKey;
		MouseButton = mouseButton;
		AltMouseButton = altMouseButton;
		ControllerButtons = controllerButtons;
	}

    public KeyBinding(KeyCode key, KeyCode altKey, MouseButtons mouseButton, MouseButtons altMouseButton)
    {
        Key = key;
        AltKey = altKey;
        MouseButton = mouseButton;
        AltMouseButton = altMouseButton;
    }

    public KeyBinding(KeyCode key, KeyCode altKey)
    {
        Key = key;
        AltKey = altKey;
        MouseButton = MouseButtons.None;
        AltMouseButton = MouseButtons.None;
    }

    public KeyBinding(GamePadButtonValues controllerButtons)
    {
        ControllerButtons = controllerButtons;
    }

    public KeyBinding(GamePadJoystickValues joysticks)
    {
        ControllerJoysticks = joysticks;
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
		Y = 32768
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
