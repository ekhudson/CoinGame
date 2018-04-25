using UnityEngine;
using UnityEngine.Events;

public class InputHandler : MonoBehaviour
{
    [SerializeField]
    private string m_ActionName = string.Empty;

    [SerializeField]
    private UnityEvent m_OnInput;   

    private void Start()
    {
        if (!string.IsNullOrEmpty(m_ActionName))
        {
            EventManager.Instance.AddHandler<UserInputEvent>(OnInputHandler);
        }
    }

    private void OnDestroy()
    {
        EventManager.Instance.RemoveHandler<UserInputEvent>(OnInputHandler);
    }

    private void OnInputHandler(object ender, UserInputEvent evt)
    {
        if (evt.KeyBind.BindingName == m_ActionName)
        {
            if (m_OnInput != null)
            {
                m_OnInput.Invoke();
            }
        }
    }
}
