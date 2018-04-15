using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu]
public class UserActionSetAsset : ScriptableObject
{
    [SerializeField]
    private List<UserActionSet> m_ActionSets = new List<UserActionSet>();

    public List<UserActionSet> ActionSets { get { return m_ActionSets; } }
}
