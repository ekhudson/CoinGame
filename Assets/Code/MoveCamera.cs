using UnityEngine;
using XInputDotNetPure;

public class MoveCamera : MonoBehaviour
{
    [SerializeField]
    private float m_MoveSpeedHorizontal = 1f;
    [SerializeField]
    private float m_MoveSpeedVertical = 1f;
    [SerializeField]
    private float m_MoveDecayTime = 1f;

    private bool m_MoveEnabled = true;

    private Vector3 mCurrentMoveVector = Vector3.zero;

    public void Update()
    {
        
    }

    public void GetKeyboardInput()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            mCurrentMoveVector += -transform.right;
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            mCurrentMoveVector += transform.right;
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            mCurrentMoveVector += transform.forward;
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            mCurrentMoveVector += -transform.forward;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            mCurrentMoveVector += Vector3.up;
        }

        if (Input.GetKey(KeyCode.C))
        {
            mCurrentMoveVector += -Vector3.up;
        }
    }

    public void GetJoystickInput()
    {

    }
}
