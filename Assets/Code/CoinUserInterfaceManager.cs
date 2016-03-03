using UnityEngine;
using System.Collections;

public class CoinUserInterfaceManager : Singleton<CoinUserInterfaceManager> 
{

    public float DefaultTimeScale = 1f;
    public bool DoSlowMo = false;
    public bool DoCurving = false;
    public float SlowMoTimeScale = 0.5f;
    public float MaxRaycastLength = 100f;

    private Ray mCurrentRay = new Ray();
    private RaycastHit mCurretRaycastHit;

    private Ray mCurrentBoardRay = new Ray();
    private RaycastHit mCurrentBoardRayHit;

    private Camera mCamera;
    private Transform mCameraTransform;

    private int mCoinLayermask = 1;
    private int mBoardLayerMask = 1;

    private GameObject mCurrentCollidingObject = null;

    private static Vector3 mLastMousePos = Vector3.zero;
    private static Vector3 mCurrentMouseDelta = Vector3.zero;

    private static Vector3 mMouseBoardPosition = Vector3.zero;

    private const float kMinTimeScale = 0.05f;
    private const float kMaxTimeScale = 1f;


    public static Vector3 MouseDeltaVector3
    {
        get
        {
            return mCurrentMouseDelta;
        }
    }

    public static Vector2 MouseDeltaVector2
    {
        get
        {
           return new Vector2(mCurrentMouseDelta.x, mCurrentMouseDelta.y);
        }
    }

    public static Vector3 MouseBoardPosition
    {
        get
        {
            return mMouseBoardPosition;
        }
    }

	// Use this for initialization
	private void Start () 
    {
        mCamera = Camera.main;
        mCameraTransform = mCamera.transform;
        mLastMousePos = Input.mousePosition;
        mBoardLayerMask = 1 << LayerMask.NameToLayer("BoardLayer");
	}
	
	// Update is called once per frame
	private void OnGUI () 
    {
        CalculateMouseDelta();
        GetBoardMousePosition();
	}

    private void GetBoardMousePosition()
    {
        mCurrentBoardRay = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        if (Physics.Raycast(mCurrentBoardRay, out mCurrentBoardRayHit, 1000f, mBoardLayerMask))
        {
            mMouseBoardPosition = mCurrentBoardRayHit.point;
        }
    }

    private void CalculateMouseDelta()
    {
        mCurrentMouseDelta = mLastMousePos - Input.mousePosition;
        mLastMousePos = Input.mousePosition;
    }
  
}
