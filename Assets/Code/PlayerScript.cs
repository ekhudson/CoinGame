using UnityEngine;
using UnityStandardAssets.ImageEffects;
using System.Collections;
using System.Collections.Generic;

public class PlayerScript : MonoBehaviour 
{
    public GameObject PlayerCoinPrefab;
    public BoxCollider CoinBounds;
    public LayerMask CoinLayerMask;
    public float CoinForceMin = 5f;
    public float CoinForceMax = 25f;
    public float ForcePerSecond = 5f;
    public Vector3 PlayerCoinStartingRotation = new Vector3(90f, 0f, 0f);
    public float PlayerCoinDistanceFromCamera = 10f;
    public float ShotDelay = 5f;
    public Vector3 ShotReloadOffset = new Vector3(0.1f, 0.1f, 0f);
    public Vector3 CoinCenterOffset = Vector3.zero;
    public float PanSpeed = 0.25f;
    public float OffsetMag = 2f;
    public float MaxAngularVelocity = 35f;
    public float StrengthBarHeight = 32f;
    public Color StrengthBarColor = Color.yellow;
    public Color StrengthBarBackgroundColor = Color.gray;
    public bool FollowCoins = true;
    public float RotationIncrementDegrees = 1f;
    public Texture2D WhiteTexture = null;
    public DepthOfField DOFScript;

    private bool mNeedReload = false;
    private Ray mRay;
    private RaycastHit mHit;

    private float mCurrentStrength = 0.5f;

    private GameObject mPreviewCoin;
    private Renderer mPreviewRenderer;
    private float mCountHeight = 0f;
    private Camera mCamera;
    private GameObject mInventoryCoin;
    private float mCoinRadius = 10f;
    private Vector3 mCustomRotation = Vector3.zero;
    private bool mGamePaused = false;

    private float mCurrentReloadTime = 0f;
    private Vector3 mCurrentReloadPosition = Vector3.zero;
    private Vector3 mPreviousForceAmount = Vector3.zero;

    private bool mMouseOnCoin = false;

    private MouseOrbitScript mOrbitScript;
    private float mDefaultFixedTimeStep;

    private GameObject mLastFiredCoin;
    private float mTimeMouseButtonHeld = 0f;
    private float mMaxHoldTime = 10f;

    private GUIStyle mReticuleStyle;

    private const float kReticuleWidth = 16f;

    private void Start()
    {
        mCamera = Camera.main;
        mPreviewCoin = (GameObject)GameObject.Instantiate(PlayerCoinPrefab);
        mPreviewRenderer = mPreviewCoin.GetComponent<MeshRenderer>();

        

        mDefaultFixedTimeStep = Time.fixedDeltaTime;

        foreach (Material mat in mPreviewRenderer.materials)
        {
            mat.color = Color.Lerp(mat.color, Color.clear, 0.45f);
        }
     
        mPreviewCoin.GetComponent<Rigidbody>().isKinematic = true;
        mPreviewCoin.transform.rotation = Quaternion.Euler(PlayerCoinStartingRotation);
        mPreviewCoin.name = "Preview Coin";

        mPreviewCoin.layer = LayerMask.NameToLayer("PreviewCoinLayer");
        
        foreach(Transform child in mPreviewCoin.transform)
        {
            child.gameObject.layer = LayerMask.NameToLayer("PreviewCoinLayer");
        }


        mCountHeight = mPreviewRenderer.bounds.extents.y;
        mCurrentReloadPosition = mCamera.ViewportToWorldPoint(ShotReloadOffset);

        mInventoryCoin = (GameObject)GameObject.Instantiate(PlayerCoinPrefab);
        mInventoryCoin.GetComponent<Rigidbody>().isKinematic = true;
        mInventoryCoin.GetComponentInChildren<Collider>().isTrigger = true;
        mInventoryCoin.transform.position = mCamera.ViewportToWorldPoint(ShotReloadOffset);

        mCoinRadius = PlayerCoinPrefab.GetComponent<Renderer>().bounds.extents.x;

        mOrbitScript = GetComponent<MouseOrbitScript>();

        EventManager.Instance.AddHandler<PlayerCoinImpactEvent>(PlayerCoinImpactEventHandler);
    }

	private void LateUpdate()
    {
        if (WhiteTexture == null)
        {
            WhiteTexture = new Texture2D(1, 1);
            WhiteTexture.SetPixel(1, 1, Color.white);
            WhiteTexture.Apply(); 
        }

        if (DOFScript.focalTransform == null && mPreviewCoin != null)
        {
            DOFScript.focalTransform = mPreviewCoin.transform;
        }

        DrawInventory();

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            mGamePaused = !mGamePaused;
        }

        if (mGamePaused)
        {
            Time.timeScale = 0f; 
        }    
        else if (Time.timeScale == 0)
        {
            Time.timeScale = 1f;

            // mCurrentStrength = Mathf.Clamp((mCurrentStrength + Input.GetAxis("Mouse ScrollWheel")), 0f, 1f);
        }

        if (!mGamePaused)
        {
            mCurrentStrength = Mathf.Clamp(mTimeMouseButtonHeld / mMaxHoldTime, 0f, 1f);
        }

        if (mNeedReload)
        {
            if (Input.GetKeyUp(KeyCode.Space) || Input.GetMouseButtonUp(0) && !mGamePaused)
            {
                mNeedReload = false;
                Reload();
            }
        } 
        else
        {    
            if (mCurrentReloadTime < ShotDelay && !mGamePaused)
            {
                mCurrentReloadTime += Time.deltaTime;

                float time = mCurrentReloadTime / ShotDelay;

                mPreviewCoin.transform.position = Vector3.Slerp(mCurrentReloadPosition, mCamera.transform.position + (mCamera.transform.forward * PlayerCoinDistanceFromCamera), time);
                mPreviewRenderer.transform.forward = Vector3.Slerp(Vector3.down, mCamera.transform.forward, time);
                mPreviewCoin.transform.rotation = Quaternion.Slerp(Quaternion.Euler(-PlayerCoinStartingRotation), Quaternion.Euler(PlayerCoinStartingRotation), time);
                return;
            }

            mPreviewCoin.transform.position = (mCamera.transform.position + (mCamera.transform.forward * PlayerCoinDistanceFromCamera));
            mPreviewCoin.transform.forward = mCamera.transform.forward;
            mPreviewCoin.transform.Rotate(PlayerCoinStartingRotation);
            mPreviewCoin.transform.rotation *= Quaternion.Euler(mCustomRotation);

            if (Input.GetKeyUp(KeyCode.C))
            {
                CoinCenterOffset = Vector3.zero;
            }

            if (Input.GetMouseButton(2))
            {
                //Pan();
            }

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                //mCustomRotation.y += -RotationIncrementDegrees;

                Vector3 right = transform.right;

                transform.position += -right * 1f;
                mOrbitScript.Target.transform.position += -right * 1f;
            }

            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                //mCustomRotation.y += RotationIncrementDegrees;

                Vector3 right = transform.right;
                
                transform.position += right * 1f;
                mOrbitScript.Target.transform.position += right * 1f;
            }

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                //mCustomRotation.x += RotationIncrementDegrees;

                Vector3 forward = Vector3.Cross(transform.right, Vector3.up);
                
                transform.position += forward * 1f;
                mOrbitScript.Target.transform.position += forward * 1f;
            }

            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                //mCustomRotation.x += -RotationIncrementDegrees;

                Vector3 forward = Vector3.Cross(transform.right, Vector3.up);
                
                transform.position += -forward * 1f;
                mOrbitScript.Target.transform.position += -forward * 1f;
            }


            Vector3 mousePos = Input.mousePosition;

            mRay = mCamera.ScreenPointToRay(mousePos);
            mHit = new RaycastHit();
            
            if (Physics.Raycast(mRay, out mHit, 100f, CoinLayerMask))
            {
                if (mHit.collider.transform == null)
                {
                    return;
                }

                if (mHit.collider.transform.gameObject != mPreviewCoin.gameObject)
                {
                    mMouseOnCoin = false;
                    return;
                }
                else
                {
                    CoinCenterOffset = mPreviewCoin.transform.InverseTransformPoint(mHit.point);
                    CoinCenterOffset.y = CoinCenterOffset.z * -1; //for some reason InverseTransformPoint puts y in z?
                    CoinCenterOffset.z = 0f;
                    mMouseOnCoin = true;
                }
            }
            else
            {
                mMouseOnCoin = false;
                CoinCenterOffset = Vector3.zero;
                //return;
            }

            if (!mGamePaused)
            {
                if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
                {
                    mMaxHoldTime = (CoinForceMax / ForcePerSecond);
                    mTimeMouseButtonHeld = 0f;
                    return;
                }

                if (Input.GetMouseButton(0) || Input.GetKeyDown(KeyCode.Space))
                {
                    mTimeMouseButtonHeld += Time.deltaTime;
                }

                if (Input.GetMouseButtonUp(0) || Input.GetKeyDown(KeyCode.Space))
                {
                    SpawnCoin();
                    mNeedReload = true;
                }
            }
        }
    }

    private void OnGUI()
    {
        DrawDebug();

        DrawStrength();

        GetInput();

        if (mNeedReload || (mCurrentReloadTime < ShotDelay) || !mMouseOnCoin)
        {
            return;
        }

        Vector3 pos = mPreviewCoin.transform.position;   

        pos += mCamera.transform.up * CoinCenterOffset.y;
        pos += mCamera.transform.right * CoinCenterOffset.x;
       
        pos = mCamera.WorldToScreenPoint(pos);   
              
        pos.y = mCamera.pixelHeight - pos.y;

        pos.y -= kReticuleWidth * 0.5f;
        pos.x -= kReticuleWidth * 0.5f;

        GUI.color = Color.red;
        GUI.DrawTexture(new Rect(pos.x, pos.y, kReticuleWidth, kReticuleWidth), WhiteTexture, ScaleMode.StretchToFill);
        GUI.color = Color.white;
    }

    private void GetInput()
    {
        if (mOrbitScript.GetState == MouseOrbitScript.CameraStates.FOLLOWING_COIN)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                Time.timeScale = 0.25f;
            }
            else
            {
                Time.timeScale = 1f;
            }
        }
    }

    private void DrawDebug()
    {
        GUILayout.BeginArea(new Rect(0, 32f, Screen.width, Screen.height));

        if (mGamePaused)
        {
            GUI.Box(new Rect(Screen.width * 0.5f, Screen.height * 0.5f, 128f, 128f), "PAUSED");


            FollowCoins = GUILayout.Toggle(FollowCoins, "Follow");
            CoinUserInterfaceManager.Instance.DoSlowMo = GUILayout.Toggle(CoinUserInterfaceManager.Instance.DoSlowMo, "Do Slow Mo");
            CoinUserInterfaceManager.Instance.DoCurving = GUILayout.Toggle(CoinUserInterfaceManager.Instance.DoCurving, "Do Curving");
        }

        GUILayout.Label("Time Scale: " + Time.timeScale.ToString());
        GUILayout.Label("Fixed Delta Time: " + Time.fixedDeltaTime.ToString());
        GUILayout.Label("Camera State: " + mOrbitScript.GetState.ToString());

        if (mLastFiredCoin != null)
        {
            GUILayout.Label("Coin Velocity: " + mLastFiredCoin.GetComponent<Rigidbody>().velocity.ToString());
            GUILayout.Label("Previous Force Amount: " + mPreviousForceAmount.ToString());
        }

        GUILayout.EndArea();
    }

    private void DrawStrength()
    {
        float maxWidth = (mCamera.pixelWidth - (StrengthBarHeight * 2));
        Rect barRect = new Rect(StrengthBarHeight, mCamera.pixelHeight - StrengthBarHeight, maxWidth, StrengthBarHeight);

        GUI.color = StrengthBarBackgroundColor;
        GUI.DrawTexture(barRect, WhiteTexture, ScaleMode.StretchToFill);

        float currentWidth = Mathf.Lerp(0f, maxWidth, mCurrentStrength);

        float force = (Mathf.Lerp(CoinForceMin, CoinForceMax, mCurrentStrength));

        GUI.Label(barRect, "Current Strength: " + System.Math.Round(force,2).ToString());

        barRect.width = currentWidth;

        GUI.color = StrengthBarColor;
        GUI.DrawTexture(barRect, WhiteTexture, ScaleMode.StretchToFill);

        GUI.color = Color.white;
    }

    private void Pan()
    {
        CoinCenterOffset.x = Mathf.Clamp(CoinCenterOffset.x + Input.GetAxis("Mouse X") * PanSpeed, -mCoinRadius, mCoinRadius);
        CoinCenterOffset.y = Mathf.Clamp(CoinCenterOffset.y + Input.GetAxis("Mouse Y") * PanSpeed, -mCoinRadius, mCoinRadius);
        CoinCenterOffset = Vector3.ClampMagnitude(CoinCenterOffset, mCoinRadius);
    }

    private void DrawInventory()
    {
        mInventoryCoin.transform.position = mCamera.ViewportToWorldPoint(ShotReloadOffset);
        mInventoryCoin.transform.forward = mCamera.transform.forward;
        mInventoryCoin.transform.Rotate(-PlayerCoinStartingRotation);
    }

    private void Reload()
    {
        mCurrentStrength = 0f;
        mLastFiredCoin.GetComponent<PlayerCoinScript>().FreshCoin = false;
        mOrbitScript.SetState(MouseOrbitScript.CameraStates.IDLE);
        Time.timeScale = 1f;
        //Time.fixedDeltaTime = mDefaultFixedTimeStep;
        mCurrentReloadTime = 0;
        mCurrentReloadPosition = mCamera.ViewportToWorldPoint(ShotReloadOffset);
        mPreviewCoin.transform.position = mCurrentReloadPosition;
        mPreviewRenderer.enabled = true;
        mCoinRadius = PlayerCoinPrefab.GetComponent<Renderer>().bounds.extents.x;
        DOFScript.focalTransform = mPreviewCoin.transform;
    }

    private void OnDrawGizmos()
    {
        if (mCamera == null)
        {
            return;
        }

        Gizmos.color = Color.green;
        Gizmos.DrawLine(mCamera.transform.position, mCamera.transform.position + (mCamera.transform.forward * 10f));
        Gizmos.color = Color.white;

        Vector3 force = mCamera.transform.forward * CoinForceMax;
        Vector3 forcePos = mPreviewCoin.transform.position;
        forcePos += mPreviewCoin.transform.right * (CoinCenterOffset.x * OffsetMag);
        forcePos += mPreviewCoin.transform.up * (CoinCenterOffset.y * OffsetMag);
        forcePos -= (mCamera.transform.forward *  2);

        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(forcePos, 0.05f);

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(forcePos, forcePos + force);
        Gizmos.color = Color.white;
    }

    private void SpawnCoin()
    {
        Vector3 pos = mPreviewCoin.transform.position;
        Quaternion rot = mPreviewCoin.transform.rotation;

        mPreviewRenderer.enabled = false;

        GameObject playerCoin = (GameObject)GameObject.Instantiate(PlayerCoinPrefab, pos, rot);
        playerCoin.GetComponent<Rigidbody>().maxAngularVelocity = MaxAngularVelocity;

        

        Vector3 centerOffset = Vector3.zero;

        float magX = Mathf.Lerp(0f, OffsetMag, Mathf.Abs(CoinCenterOffset.x) * 2);
        float magY = Mathf.Lerp(0f,OffsetMag, Mathf.Abs(CoinCenterOffset.y) * 2);

        centerOffset += mCamera.transform.right * (CoinCenterOffset.x * magX);
        centerOffset += mCamera.transform.up * (CoinCenterOffset.y * magY);

        Vector3 force = (mPreviewCoin.transform.position - mCamera.transform.position).normalized; //mCamera.transform.forward * (Mathf.Lerp(CoinForceMin, CoinForceMax, mCurrentStrength));

        force *= (Mathf.Lerp(CoinForceMin, CoinForceMax, mCurrentStrength));

        mPreviousForceAmount = force;

        mLastFiredCoin = playerCoin;

        playerCoin.GetComponent<Rigidbody>().AddForceAtPosition(force, playerCoin.transform.position + centerOffset - (mCamera.transform.forward * 0.25f), ForceMode.VelocityChange);

        playerCoin.name = "Launched Coin";

        DOFScript.focalTransform = playerCoin.transform;

        PlayerCoinScript playerCoinScript = playerCoin.GetComponent<PlayerCoinScript>();

        playerCoinScript.LaunchCoin();        

        if (FollowCoins)
        {
            mOrbitScript.SetState(MouseOrbitScript.CameraStates.FOLLOWING_COIN);
            mOrbitScript.SetTarget(playerCoin.transform);

            if (CoinUserInterfaceManager.Instance.DoSlowMo)
            {
                Time.timeScale = 0.45f;
            }
            //Time.fixedDeltaTime = mDefaultFixedTimeStep * (Time.timeScale / 1f);
        }


    }

    public void PlayerCoinImpactEventHandler(object sender, PlayerCoinImpactEvent impactEvent)
    {
        if (FollowCoins && mOrbitScript.SecondaryFollowPoint == null && impactEvent.PlayerCoin.FreshCoin)
        {
            //impactEvent.PlayerCoin.FreshCoin = false;
            //mOrbitScript.SetSecondaryFollowPoint(impactEvent.CollisionData.transform);
            mOrbitScript.NewSecondFollowPoint(impactEvent.CollisionData.contacts[0].point);
        }
    }
}
