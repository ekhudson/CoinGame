using UnityEngine;

[System.Serializable]
public class UserActionDefinition
{
    [SerializeField]
    private string m_Name;
    [SerializeField]
    private KeyBinding m_KeyBinding;

    public string Name { get { return m_Name; } }
    public KeyBinding Binding { get { return m_KeyBinding; } }
}
