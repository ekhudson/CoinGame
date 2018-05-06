﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PogManager : Singleton<PogManager>
{
    [SerializeField]
    private bool m_DebugMode = false;

    [Header("Gameplay Options")]
    [SerializeField]
    private bool m_ReStackEachTurn = false;
    [SerializeField]
    private bool m_RemoveThrownSlammers = false;

    [Header("Stack Spawn Settings")]
    [SerializeField] private GameObject m_CoinPrefab;
    [SerializeField] private int m_MinCoinAmount = 10;
    [SerializeField] private int m_MaxCoinAmount = 10;
    [SerializeField] private float m_StartDelayMin = 0.1f;
    [SerializeField] private float m_StartDelayMax = 0.4f;
    [SerializeField] private float m_SpawnDelay = 0.25f;
    [SerializeField] private float m_MaxOffset = 0.5f;
    [SerializeField] private float m_MaxRotation = 90f;

    private Dictionary<int, List<CoinScript>> mPlayerToPogDictionary = new Dictionary<int, List<CoinScript>>();
    [SerializeField]
    [ReadOnly]
    private List<CoinScript> mPogsInPlay = new List<CoinScript>();
    private List<CoinScript> mActivePogs = new List<CoinScript>();
    private Transform mSpawnPosition;
    private int mCurrentSpawnCount = 0;
    private int mSpawnAmount = 0;
    private float mStartDelay = 0f;
    private float mCurrentStartDelay = 0f;
    private float mCurrentSpawnDelay = 0f;
    private float mCoinHeight = 0f;
    private float mCurrentTotalHeight = 0f;
    private Vector3 mLastHitPoint = Vector3.zero;
    private Vector3 mLastSpawnPoint = Vector3.zero;
    private PlayerCoinScript mLastThrownSlammer = null;
    private bool mIsSpawning = false;
    private float mPostLaunchTime = 0f;

    private const float kPostLaunchTurnEndDelay = 0.25f;

    public PlayerCoinScript LastThrownSlammer
    {
        get
        {
            return mLastThrownSlammer;
        }
        set
        {
            mLastThrownSlammer = value;
        }
    }

    public int RemainingPogsCount
    {
        get
        {
            return mPogsInPlay.Count;
        }
    }

    private void Start()
    {
        EventManager.Instance.AddHandler<CoinEvent>(OnCoinEvent); if (m_CoinPrefab == null)
        {
            return;
        }

        if (m_CoinPrefab != null && m_CoinPrefab.GetComponent<CoinScript>() != null)
        {
            mCoinHeight = m_CoinPrefab.GetComponent<CoinScript>().CoinMeshRenderer.bounds.size.y * m_CoinPrefab.transform.lossyScale.y;
        }
        else
        {
            Debug.LogWarning("No Pog Prefab Specified", this);
        }

        mStartDelay = Random.Range(m_StartDelayMin, m_StartDelayMax);
        mSpawnAmount = Random.Range(m_MinCoinAmount, m_MaxCoinAmount);
        StartCoroutine(DoStartDelay(mStartDelay));
    }

    public void SetSpawnPosition(Transform spawnPosition)
    {
        mSpawnPosition = spawnPosition;
    }

    private void CheckIfTurnEnded()
    {
        if (SessionManager.Instance.CurrentTurnState == SessionTurn.TurnStates.Launched && 
            mActivePogs.Count == 0 &&
            !mLastThrownSlammer.BaseRigidbody.IsSleeping())
        {
            if ( (m_RemoveThrownSlammers) && (mLastThrownSlammer != null))
            {
                Destroy(mLastThrownSlammer.gameObject);
            }

            SessionManager.Instance.EndCurrentTurn();
            mPostLaunchTime = 0f;
        }
    }

    private void CheckIfRoundEnded()
    {
        if (mPogsInPlay.Count <= 0)
        {
            SessionManager.Instance.EndSession();
        }
    }
    
    private void OnCoinEvent(object sender, CoinEvent coinEvent)
    {
        if (coinEvent.CoinEventType == CoinEvent.CoinEventTypes.IMPACTED)
        {
           // mCurrentActiveCount++;
        }
        else if (coinEvent.CoinEventType == CoinEvent.CoinEventTypes.SETTLED_FACE_DOWN)
        {
            //mCurrentActiveCount--;
            if (mPostLaunchTime > kPostLaunchTurnEndDelay)
            {
                CheckIfTurnEnded();
            }
        }
        else if (coinEvent.CoinEventType == CoinEvent.CoinEventTypes.SETTLED_FACE_UP)
        {
            mPogsInPlay.Remove(coinEvent.Coin);
            //mCurrentActiveCount--;
            CheckIfRoundEnded();
        }
    }

    private void Update () 
    {
        if (SessionManager.Instance.CurrentTurnState == SessionTurn.TurnStates.Launched)
        {
            foreach(CoinScript pog in mPogsInPlay)
            {
                if ( (!pog.CoinRigidbody.IsSleeping()) && (!mActivePogs.Contains(pog)) )
                {
                    mActivePogs.Add(pog);
                }
                else if ( (pog.CoinRigidbody.IsSleeping()) && (mActivePogs.Contains(pog)))
                {
                    mActivePogs.Remove(pog);
                }
            }

            mPostLaunchTime += Time.deltaTime;
        }
        else
        {
            mPostLaunchTime = 0f;
        }
	}

    private void OnGUI()
    {
        if (m_DebugMode)
        {
            GUILayout.BeginVertical();

            GUILayout.Label("Pog in play: " + mPogsInPlay.Count.ToString());
            GUILayout.Label("Active Pogs: " + mActivePogs.Count.ToString());

            GUILayout.EndVertical();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;

        Gizmos.DrawSphere(mLastHitPoint, 0.05f);

        Gizmos.color = Color.white;

        Gizmos.DrawSphere(mLastSpawnPoint, 0.05f);

        Gizmos.DrawLine(mLastSpawnPoint, mLastHitPoint);
    }

    private void SpawnPog()
    {
        Vector3 position = mSpawnPosition.position;

        RaycastHit hit;
        Ray testRay = new Ray(mSpawnPosition.position + (Vector3.up * mCurrentSpawnCount) + (Vector3.up * 0.5f), Vector3.down);

        if (Physics.Raycast(testRay, out hit, 100f))
        {
            mLastHitPoint = hit.point;
            position = hit.point + new Vector3(0f, mCoinHeight * 0.5f, 0f);
        }

        mLastSpawnPoint = position;

        Vector3 positionOffset = Vector3.zero;

        positionOffset += Vector3.right * Random.Range(-m_MaxOffset, m_MaxOffset);
        positionOffset += Vector3.forward * Random.Range(-m_MaxOffset, m_MaxOffset);

        position += positionOffset;

        Vector3 rot = new Vector3(0f, Random.Range(-m_MaxRotation, m_MaxRotation), 0f);

        CoinScript newCoin = Instantiate(m_CoinPrefab, position, Quaternion.Euler(rot)).GetComponent<CoinScript>();

        if (newCoin != null)
        {
            newCoin.transform.parent = transform;
            mPogsInPlay.Add(newCoin);
        }
    }

    IEnumerator DoStartDelay(float delay)
    {
        if ((mSpawnPosition == null) || (SessionManager.Instance.CurrentSessionState != SessionManager.SessionStates.InProgress))
        {
            yield return null;
        }

        yield return new WaitForSeconds(delay);
        mCurrentSpawnCount = 0;
        StartCoroutine(DoSpawning(m_SpawnDelay));
    }

    IEnumerator DoSpawning(float spawnDelay)
    {
        while (mCurrentSpawnCount < mSpawnAmount)
        {
            SpawnPog();
            mCurrentSpawnCount++;
            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
