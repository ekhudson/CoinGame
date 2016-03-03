using UnityEngine;
using System.Collections;

public class CustomMouseCursor : Singleton<CustomMouseCursor>
{
    private Transform mTransform;
    private Camera mMainCamera;
    private bool mShowCursor = true;
    private UISprite mSprite;

    private const float kMouseForwardOffest = 0.01f;

    public bool ShowCursor
    {
        get
        {
            return mShowCursor;
        }
        set
        {
            mShowCursor = value;
        }
    }


    private void Start()
    {
        mTransform = transform;
        mMainCamera = Camera.main;
        mSprite = GetComponent<UISprite>();
    }

    private void OnGUI()
    {
        if (mShowCursor && !mSprite.enabled)
        {
            mSprite.enabled = true;
        }
        else if (!mShowCursor && mSprite.enabled)
        {
            mSprite.enabled = false;
        }

        Event e = Event.current;

        Vector3 mousePos = new Vector3(e.mousePosition.x, Screen.height - e.mousePosition.y, 1f);

        mTransform.position = mMainCamera.ScreenToWorldPoint(mousePos + (Vector3.forward * (mMainCamera.nearClipPlane + kMouseForwardOffest)));
        mTransform.rotation = mMainCamera.transform.rotation;
    }

    private void ToggleCursor()
    {
        mShowCursor = !mShowCursor;
    }
}
