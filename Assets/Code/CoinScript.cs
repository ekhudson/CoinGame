﻿using UnityEngine;
using System.Collections;

public class CoinScript : BaseObject 
{
    public enum CoinStates
    {
        IDLE,
        HIGHLIGHTED,
        GRABBED,
    }

    public bool IsPlayerCoin = false;
    public MeshRenderer CoinMeshRenderer;
    public Rigidbody CoinRigidbody;
    public float MaxVelocity;
    public int Value = 1;
    public ParticleSystem CoinDespawnEffect;

    private const float kFacingUpThreshold = -0.8f;

    private CoinStates mCoinState = CoinStates.IDLE;
    private Rigidbody mRigidbody;
    private float mCurrentFacingDot = 0f;
    private bool mAsleepLastFrame = false;

    private void Start()
    {
        mRigidbody = CoinRigidbody;
    }

    private void FixedUpdate()
    {
        if (mRigidbody.velocity.sqrMagnitude > MaxVelocity * MaxVelocity)
        {
            mRigidbody.velocity = Vector3.ClampMagnitude(mRigidbody.velocity, MaxVelocity);
        }

        mCurrentFacingDot = Vector3.Dot(BaseTransform.up, Vector3.up);

        if (mRigidbody.IsSleeping() && !IsPlayerCoin)
        {
            if (mCurrentFacingDot < kFacingUpThreshold)
            {
                RemoveCoin();
            }
            else
            {
                EventManager.Instance.Post(new CoinEvent(this, this, CoinEvent.CoinEventTypes.SETTLED_FACE_DOWN));
            }
        }
    }

    private void RemoveCoin()
    {
        EventManager.Instance.Post(new CoinEvent(this, this, CoinEvent.CoinEventTypes.SETTLED_FACE_UP));

        if (CoinDespawnEffect != null)
        {
            CoinDespawnEffect.transform.position = transform.position;
            CoinDespawnEffect.transform.parent = null;
            CoinDespawnEffect.Play();
        }

        BaseGameObject.SetActive(false);
        this.enabled = false;
    }
}
