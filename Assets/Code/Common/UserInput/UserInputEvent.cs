using UnityEngine;
using System.Collections;

using XInputDotNetPure;

public class UserInputEvent : EventBase
{
    public enum TYPE
    {
        KEYDOWN,
        KEYHELD,
        KEYUP,
		GAMEPAD_BUTTON_DOWN,
		GAMEPAD_BUTTON_HELD,
		GAMEPAD_BUTTON_UP,
		GAMEPAD_JOYSTICK,
    }
    
    public readonly TYPE Type;
    public readonly KeyBinding KeyBind;
	public readonly JoystickInfoClass JoystickInfo;
	public readonly GamePadInfoClass GamePadInfo;
	public readonly int PlayerIndexInt = -1;
    
	public class JoystickInfoClass
	{
		public readonly float AmountX = 0.0f;
		public readonly float AmountY = 0.0f;
        public readonly Vector2 Value = Vector2.zero;

		public JoystickInfoClass(float amountX, float amountY)
		{		
			AmountX = amountX;
			AmountY = amountY;
            Value.x = AmountX;
            Value.y = AmountY;

        }

        public JoystickInfoClass(Vector2 value)
        {
            Value = value;
            AmountX = Value.x;
            AmountY = Value.y;
        }
	}

	public class GamePadInfoClass
	{
		public readonly GamePadState PadState;

		public GamePadInfoClass(GamePadState gamePadState)
        {
			PadState = gamePadState;
		}
	}

	public UserInputEvent(UserInputEvent.TYPE inputType, KeyBinding bind, Vector3 location, object sender) : base(location, sender)
	{
		Type = inputType;
		KeyBind = bind;
	}

    public UserInputEvent(UserInputEvent.TYPE inputType, KeyBinding bind, JoystickInfoClass joystickInfo, Vector3 location, object sender) : base(location, sender)
    {
        Type = inputType;
        KeyBind = bind;
		JoystickInfo = joystickInfo;
    }

	public UserInputEvent(UserInputEvent.TYPE inputType, KeyBinding bind, JoystickInfoClass joystickInfo, int playerIndex, Vector3 location, object sender) : base(location, sender)
	{
		Type = inputType;
		KeyBind = bind;
		JoystickInfo = joystickInfo;
		PlayerIndexInt = playerIndex;
	}

	public UserInputEvent(UserInputEvent.TYPE inputType, KeyBinding bind, int playerIndex, Vector3 location, object sender) : base(location, sender)
	{
		Type = inputType;
		KeyBind = bind;
		PlayerIndexInt = playerIndex;
	}
}