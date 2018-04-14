using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu]
public class KeyBindingsAsset : ScriptableObject
{
    [SerializeField]
    private List<KeyBinding> m_KeyBindings = new List<KeyBinding>();

    public List<KeyBinding> KeyBindings { get { return m_KeyBindings; } }
}
