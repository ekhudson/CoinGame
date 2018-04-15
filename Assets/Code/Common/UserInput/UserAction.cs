using UnityEngine;

[System.Serializable]
public class UserAction
{
    public enum ActionStates
    {
        NONE,
        PRESSED,
        RELEASED,
    }

    private string mName;
    private KeyBinding mKeyBinding;
    private ActionStates mActionState = ActionStates.NONE;
    private float mFloatValue = 0f;
    private Vector2 mVector2Value = Vector2.zero;
    private bool mEnabled = true;

    public string Name { get { return mName; } }
    public KeyBinding Binding { get { return mKeyBinding; } }
    public ActionStates State { get { return mActionState; } set { mActionState = value; } }
    public float FloatValue { get { return mFloatValue; }  set { mFloatValue = value; } }
    public Vector2 Vector2Value { get { return mVector2Value; } set { mVector2Value = value; } }
    public bool Enabled { get { return mEnabled; } set { mEnabled = value; } }

    public UserAction (string name, KeyBinding binding)
    {
        mName = name;
        mKeyBinding = binding;
    }
}
