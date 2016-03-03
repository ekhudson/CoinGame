using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovableObject : MonoBehaviour
{

    //public float MoveSpeed = 1f;

    private bool mInterruptable = true;

    private int mTargetPosition = -1;
    //private int mPreviousPosition = -1;
    //private int mCurrentPosition;

    private Vector3 mMoveDirection = Vector3.zero;
    private float mMoveSpeed = 1f;
    private Vector3 mOriginalPosition = Vector3.zero;
    private Vector3 mOriginalSize = Vector3.zero; //used for drawing the future position
    private Quaternion mOriginalRotation = Quaternion.identity; //used for drawing hte future positions
    private float mCurrentMoveTime = 0f;
    private List<MovableObjectController.MovableObjectPosition> mPositionQueue = new List<MovableObjectController.MovableObjectPosition>();

    private float mOriginalMagnitude;
    private int mIterateDirection = 1;

    private const float kMinimumStopDistance = 0.01f;

    public enum LoopModes
    {
        NONE,
        PINGPONG,
        WRAP,
    }

    private LoopModes mLoopMode;

    public enum MoveModes
    {
        LINEAR,
        LERP,
    }

    private MoveModes mMoveMode;

    public enum MoveableObjectStates
    {
        IDLE,
        MOVING,
    }

    public MoveableObjectStates State;

    public bool IsInterruptable
    {
        get
        {
            return mInterruptable;
        }
    }

    public Vector3 OriginalPosition
    {
        get
        {
            return mOriginalPosition;
        }
    }

    public Quaternion OriginalRotation
    {
        get
        {
            return mOriginalRotation;
        }
    }

    public Vector3 OriginalSize
    {
        get
        {
            return mOriginalSize;
        }
    }

    public MovableObjectController.MovableObjectPosition CurrentTargetPosition
    {
        get
        {
            if (mPositionQueue.Count <= 0 || mTargetPosition < 0)
            {
                return null;
            }
            else
            {
                return mPositionQueue[mTargetPosition];
            }
        }
    }

	// Use this for initialization
	private void Start ()
    {
        mOriginalPosition = transform.position;
        mOriginalSize = GetComponent<Renderer>().bounds.size;
        mOriginalRotation = transform.rotation;
	}

    private void Update()
    {
        switch(State)
        {
            case MoveableObjectStates.IDLE:

            break;

            case MoveableObjectStates.MOVING:

                if (mMoveMode == MoveModes.LERP)
                {
                    transform.position = Vector3.Lerp(transform.position, mOriginalPosition + mPositionQueue[mTargetPosition].Position, mMoveSpeed * Time.deltaTime);
                    transform.rotation = Quaternion.Lerp(transform.rotation, mPositionQueue[mTargetPosition].Rotation, mMoveSpeed * Time.deltaTime);
                }
                else if (mMoveMode == MoveModes.LINEAR)
                {
                    transform.position += (mMoveDirection * (mMoveSpeed * Time.deltaTime));
                    transform.rotation = Quaternion.Lerp(transform.rotation, mPositionQueue[mTargetPosition].Rotation, (1 - ((mOriginalPosition + mPositionQueue[mTargetPosition].Position) - transform.position).magnitude / mOriginalMagnitude));
                }

                if ( ((mOriginalPosition + mPositionQueue[mTargetPosition].Position) - transform.position).magnitude < kMinimumStopDistance)
                {
                    transform.position = mOriginalPosition + mPositionQueue[mTargetPosition].Position;
                    transform.rotation = mPositionQueue[mTargetPosition].Rotation;

                    mTargetPosition += mIterateDirection;

                    if (mTargetPosition >= mPositionQueue.Count || mTargetPosition < 0)
                    {
                        mCurrentMoveTime = 0;
                        //mCurrentPosition = mTargetPosition;

                        if (mLoopMode == LoopModes.NONE)
                        {
                            mTargetPosition = -1;
                            State = MoveableObjectStates.IDLE;
                            return;
                        }
                        else if (mLoopMode == LoopModes.WRAP)
                        {
                            mTargetPosition = mTargetPosition < 0 ? mPositionQueue.Count - 1 : 0;
                        }
                        else if (mLoopMode == LoopModes.PINGPONG)
                        {
                            mIterateDirection *= -1;
                            mTargetPosition += mIterateDirection;
                        }
                    }

                    mMoveDirection = ((mOriginalPosition + mPositionQueue[mTargetPosition].Position) - transform.position);
                    mOriginalMagnitude = mMoveDirection.magnitude;
                    mMoveDirection = mMoveDirection.normalized;
                }

                mCurrentMoveTime += Time.deltaTime;

            break;
        }
    }

    public void GivePath(List<MovableObjectController.MovableObjectPosition> positions, float speed, bool interruptable, MoveModes moveMode, LoopModes loopMode)
    {
        if (State == MoveableObjectStates.MOVING && !mInterruptable)
        {
            return;
        }

        mPositionQueue.Clear();
        mPositionQueue = positions;
        mMoveSpeed = speed;
        mMoveMode = moveMode;
        mLoopMode = loopMode;

        mMoveDirection = ((mOriginalPosition + mPositionQueue[0].Position) - transform.position);
        mOriginalMagnitude = mMoveDirection.magnitude;
        mMoveDirection = mMoveDirection.normalized;

        mCurrentMoveTime = 0f;
        mInterruptable = interruptable;
        //mPreviousPosition = mCurrentPosition;
        //mCurrentPosition = -1;
        mTargetPosition = 0;
        State = MoveableObjectStates.MOVING;
    }

    /// <summary>
    /// Move Object directly to position.
    /// </summary>
    /// <param name='position'>
    /// Position.
    /// </param>
    public void MoveToPosition(MovableObjectController.MovableObjectPosition position, float speed)
    {
        MoveToPosition(position, speed, true);
    }

    /// <summary>
    /// Move Object directly to position.
    /// </summary>
    /// <param name='position'>
    /// Position.
    /// </param>
    /// <param name='interruptable'>
    /// Interruptable.
    /// </param>
    public void MoveToPosition(MovableObjectController.MovableObjectPosition position, float speed, bool interruptable)
    {
        if (State == MoveableObjectStates.MOVING && !mInterruptable)
        {
            return;
        }

        mPositionQueue.Clear();
        mPositionQueue.Add(position);
        mMoveSpeed = speed;
        mCurrentMoveTime = 0f;
        mInterruptable = interruptable;
        //mPreviousPosition = mCurrentPosition;
        //mCurrentPosition = -1;
        mTargetPosition = 0;
        State = MoveableObjectStates.MOVING;
    }

    private void OnDrawGizmosSelected()
    {
//        if (Application.isEditor && !Application.isPlaying)
//        {
//            mOriginalPosition = transform.position;
//            mOriginalSize = renderer.bounds.size;
//            mOriginalRotation = transform.rotation;
//        }

//        foreach(MoveableObjectPosition position in Positions)
//        {
//            Gizmos.DrawWireCube(mOriginalPosition + position.Position, position.Rotation * (mOriginalRotation * mOriginalSize));
//        }
    }
}
