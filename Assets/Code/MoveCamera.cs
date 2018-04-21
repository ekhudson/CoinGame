using UnityEngine;
using XInputDotNetPure;

public class MoveCamera : BaseObject
{
    public enum CameraStates
    {
        IDLE,
        FOLLOWING_SLAM,
    }

    [Header("Movement")]
    [SerializeField]
    private Transform m_CameraTransform;
    [SerializeField]
    private float m_MaxMoveSpeedHorizontal = 1f;
    [SerializeField]
    private float m_MaxMoveSpeedVertical = 1f;
    [SerializeField]
    private float m_MoveAccelHorizontal = 1f;
    [SerializeField]
    private float m_MoveAccelVertical = 1f;
    [SerializeField]
    private float m_MoveDecay = 0.9f;

    [Header("Rotation")]
    [SerializeField]
    private float m_RotationMaxSpeedHorizontal = 250.0f;
    [SerializeField]
    private float m_RotationMaxSpeedVertical = 120.0f;
    [SerializeField]
    private float m_RotationAccelHorizontal = 50f;
    [SerializeField]
    private float m_RotationAccelVertical = 50f;
    [SerializeField]
    private float m_RotationDecay = 0.95f;
    [SerializeField]
    private float m_MinVerticalAngle = -20f;
    [SerializeField]
    private float m_MaxVerticalAngle = 80f;

    private bool m_MoveEnabled = true;
    private bool m_RotationEnabled = true;
    private CameraStates mCameraState;
    private Transform mCurrentTarget;
    private bool mCustomFollowOrbit = false;

    private Vector3 mCurrentMoveVector = Vector3.zero;
    private Vector2 mCurrentRotationVector = Vector2.zero;
    private Vector2 mCurrentRotationInputVector = Vector2.zero;

    private const float kStopThreshold = 0.005f;

    public CameraStates GetState
    {
        get
        {
            return mCameraState;
        }
    }

    private void Start()
    {
        EventManager.Instance.AddHandler<UserInputEvent>(InputHandler);
    }

    public void Update()
    {
        UpdateRotation();
        UpdateMovement();
    }

    public void UpdateRotation()
    {
        if (!m_RotationEnabled)
        {
            return;
        }

        Quaternion newRotation = m_CameraTransform.rotation;

        mCustomFollowOrbit = (mCameraState == CameraStates.FOLLOWING_SLAM);
        
        mCurrentRotationVector.x += (mCurrentRotationInputVector.x * m_RotationAccelHorizontal) * Time.unscaledDeltaTime;
        mCurrentRotationVector.y -= (mCurrentRotationInputVector.y * m_RotationAccelVertical) * Time.unscaledDeltaTime;

        mCurrentRotationVector.x = Mathf.Clamp(mCurrentRotationVector.x, -m_RotationMaxSpeedHorizontal, m_RotationMaxSpeedHorizontal);
        mCurrentRotationVector.y = Mathf.Clamp(mCurrentRotationVector.y, -m_RotationMaxSpeedVertical, m_RotationMaxSpeedVertical);

        Vector3 newRotationEuler = m_CameraTransform.rotation.eulerAngles + new Vector3(mCurrentRotationVector.y, mCurrentRotationVector.x, 0f);

        newRotationEuler.x = ClampAngle(newRotationEuler.x, m_MinVerticalAngle, m_MaxVerticalAngle);
        newRotation = Quaternion.Euler(newRotationEuler);

        m_CameraTransform.rotation = newRotation;

        if (mCameraState == CameraStates.FOLLOWING_SLAM)
        {
            if (mCustomFollowOrbit)
            {
                return;
            }

            m_CameraTransform.rotation = Quaternion.RotateTowards(BaseTransform.rotation, Quaternion.LookRotation((mCurrentTarget.position - m_CameraTransform.position).normalized), 2f);
        }
        else
        {
            m_CameraTransform.rotation = newRotation;
        }

        if (mCurrentRotationVector != Vector2.zero)
        {
            mCurrentRotationVector *= m_RotationDecay;

            if (mCurrentRotationVector.magnitude <= kStopThreshold)
            {
                mCurrentRotationVector = Vector2.zero;
            }
        }
    }

    public void UpdateMovement()
    {
        if (!m_MoveEnabled)
        {
            return;
        }

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
    }

    public void InputHandler(object sender, UserInputEvent evt)
    {
        Vector3 moveValue = Vector3.zero;
        Vector3 rotationValue = Vector3.zero;

        if (evt.KeyBind.BindingName == "JoystickLeft")
        {
            float xAmount = evt.JoystickInfo.AmountX;
            float yAmount = evt.JoystickInfo.AmountY;

            moveValue.z += (yAmount * m_MoveAccelHorizontal) * Time.deltaTime;
            moveValue.x += (xAmount * m_MoveAccelHorizontal) * Time.deltaTime;
        }

        if (evt.KeyBind.BindingName == "JoystickRight")
        {
            if (evt.Type == UserInputEvent.TYPE.GAMEPAD_JOYSTICK)
            {
                mCurrentRotationInputVector.x = evt.JoystickInfo.AmountX;
                mCurrentRotationInputVector.y = evt.JoystickInfo.AmountY;
            }
            else if (evt.Type == UserInputEvent.TYPE.MOUSE_MOVE)
            {
                mCurrentRotationInputVector.x = Mathf.Clamp(evt.MouseDelta.AmountX, -1f, 1f);
                mCurrentRotationInputVector.y = Mathf.Clamp(evt.MouseDelta.AmountY, -1f, 1f);
            }
        }

        if (evt.KeyBind.BindingName == "Forward")
        {
            moveValue.z += (m_MoveAccelHorizontal) * Time.deltaTime;
        }
        
        if (evt.KeyBind.BindingName == "Back")
        {
            moveValue.z += (-m_MoveAccelHorizontal) * Time.deltaTime;
        }

        if (evt.KeyBind.BindingName == "Left")
        {
            moveValue.x += (-m_MoveAccelHorizontal) * Time.deltaTime;
        }        

        if (evt.KeyBind.BindingName == "Right")
        {
            moveValue.x += (m_MoveAccelHorizontal) * Time.deltaTime;
        }

        if (evt.KeyBind.BindingName == "Raise")
        {
            moveValue.y += (m_MoveAccelVertical) * Time.deltaTime;
        }

        if (evt.KeyBind.BindingName == "Lower")
        {
            moveValue.y += (-m_MoveAccelVertical) * Time.deltaTime;
        }

        if (moveValue != Vector3.zero)
        {
            mCurrentMoveVector = Vector3.Min(mCurrentMoveVector + moveValue, new Vector3(m_MaxMoveSpeedHorizontal, m_MaxMoveSpeedVertical, m_MaxMoveSpeedHorizontal));
        }
    }

    public void FollowSlammer(Transform slammerTransform)
    {
        if ( (slammerTransform != null) && (mCameraState == CameraStates.IDLE) )
        {
            mCameraState = CameraStates.FOLLOWING_SLAM;
            mCurrentTarget = slammerTransform;
        }
    }

    public void SetToIdle()
    {
        if (mCameraState != CameraStates.IDLE)
        {
            mCurrentTarget = null;
            mCameraState = CameraStates.IDLE;
        }
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}
