using UnityEngine;
using System.Collections;

public class MoveableCube : MonoBehaviour 
{
    public float MoveSpeed = 5f;
    public float MaxVelocty = 5f;
    public ForceMode ForceModeToUse = ForceMode.Acceleration;
    public float MoveDecay = 0.95f;
    public float StopThreshold = 0.1f;

    private Rigidbody mRigidbody;
    private Transform mTransform;

    private void Start()
    {
        mRigidbody = GetComponent<Rigidbody>();
        mTransform = transform;
    }

	private void FixedUpdate()
    {
        bool noInput = true;

        if (mRigidbody.velocity.magnitude < MaxVelocty)
        {

            if (Input.GetKey(KeyCode.A))
            {
                Move(-mTransform.right * (MoveSpeed * Time.deltaTime));
                noInput = false;
            } 

            if (Input.GetKey(KeyCode.D))
            {
                Move(mTransform.right * (MoveSpeed * Time.deltaTime));
                noInput = false;
            }

            if (Input.GetKey(KeyCode.W))
            {
                Move(mTransform.forward * (MoveSpeed * Time.deltaTime));
                noInput = false;
            }

            if (Input.GetKey(KeyCode.S))
            {
                Move(-mTransform.forward * (MoveSpeed * Time.deltaTime));
                noInput = false;
            }
        }

        if (noInput)
        {
            mRigidbody.velocity *= MoveDecay;

            if (mRigidbody.velocity.magnitude < StopThreshold)
            {
                mRigidbody.velocity = Vector3.zero;
            }
        }

    }

    private void Move(Vector3 velocity)
    {
        mRigidbody.AddRelativeForce(velocity, ForceModeToUse);
    }
}
