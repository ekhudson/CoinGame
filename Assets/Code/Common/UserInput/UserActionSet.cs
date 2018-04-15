using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class UserActionSet
{
    [SerializeField]
    private string m_SetName = string.Empty;
    [SerializeField]
    private List<UserActionDefinition> m_Actions = new List<UserActionDefinition>();

    public string SetName { get { return m_SetName; } }
    public List<UserActionDefinition> Actions { get { return m_Actions; } }
}
