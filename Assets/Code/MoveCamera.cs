using UnityEngine;
using XInputDotNetPure;

public class MoveCamera : MonoBehaviour
{
    [SerializeField]
    private Transform m_CameraTransform;
    [SerializeField]
    private float m_MoveSpeedHorizontal = 1f;
    [SerializeField]
    private float m_MoveSpeedVertical = 1f;
    [SerializeField]
    private float m_AccelHorizontal = 1f;
    [SerializeField]
    private float m_AccelVertical = 1f;
    [SerializeField]
    private float m_MoveDecay = 0.9f;

    private bool m_MoveEnabled = true;

    private Vector3 mCurrentMoveVector = Vector3.zero;

    private const float kStopThreshold = 0.005f;

    private void Start()
    {
        EventManager.Instance.AddHandler<UserInputEvent>(InputHandler);
    }

    public void Update()
    {
        m_CameraTransform.position += (m_CameraTransform.forward * mCurrentMoveVector.z);
        m_CameraTransform.position += (m_CameraTransform.right * mCurrentMoveVector.x);
        m_CameraTransform.position += (Vector3.up * mCurrentMoveVector.y);

        if (mCurrentMoveVector != Vector3.zero)
        {
            mCurrentMoveVector *= m_MoveDecay;

            if (mCurrentMoveVector.magnitude <= kStopThreshold)
            {
                mCurrentMoveVector = Vector3.zero;
            }
        }

        Debug.Log("Move: " + mCurrentMoveVector);
    }

    public void InputHandler(object sender, UserInputEvent evt)
    {
        Vector3 moveValue = Vector3.zero;

        if (evt.KeyBind.BindingName == "JoystickLeft")
        {
            float xAmount = evt.JoystickInfo.AmountX;
            float yAmount = evt.JoystickInfo.AmountY;

            moveValue.z += (yAmount * m_AccelHorizontal) * Time.deltaTime;
            moveValue.x += (xAmount * m_AccelHorizontal) * Time.deltaTime;
        }

        if (evt.KeyBind.BindingName == "Forward")
        {
            moveValue.z += (m_AccelHorizontal) * Time.deltaTime;
        }
        
        if (evt.KeyBind.BindingName == "Back")
        {
            moveValue.z += (-m_AccelHorizontal) * Time.deltaTime;
        }

        if (evt.KeyBind.BindingName == "Left")
        {
            moveValue.x += (-m_AccelHorizontal) * Time.deltaTime;
        }        

        if (evt.KeyBind.BindingName == "Right")
        {
            moveValue.x += (m_AccelHorizontal) * Time.deltaTime;
        }

        if (evt.KeyBind.BindingName == "Raise")
        {
            moveValue.y += (m_AccelVertical) * Time.deltaTime;
        }

        if (evt.KeyBind.BindingName == "Lower")
        {
            moveValue.y += (-m_AccelVertical) * Time.deltaTime;
        }

        if (moveValue != Vector3.zero)
        {
            mCurrentMoveVector = Vector3.Min(mCurrentMoveVector + moveValue, new Vector3(m_MoveSpeedHorizontal, m_MoveSpeedVertical, m_MoveSpeedHorizontal));
        }
    }
}
