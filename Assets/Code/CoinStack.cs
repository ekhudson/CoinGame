using UnityEngine;
using System.Collections;

public class CoinStack : MonoBehaviour 
{
    public GameObject CoinPrefab;
    public int MinCoinAmount = 10;
    public int MaxCoinAmount = 10;
    public float StartDelayMin = 0.1f;
    public float StartDelayMax = 0.4f;
    public float SpawnDelay = 0.25f;
    public float MaxOffset = 0.5f;
    public float MaxRotation = 90f;

    private int mCurrentSpawnCount = 0;
    private int mSpawnAmount = 0;
    private float mStartDelay = 0f;
    private float mCurrentStartDelay = 0f;
    private float mCurrentSpawnDelay = 0f;
    private float mCoinHeight = 0f;
    private Vector3 mPosition = Vector3.zero;
    private float mCurrentTotalHeight = 0f;

	// Use this for initialization
	private void Start () 
    {
	    if (CoinPrefab == null)
        {
            return;
        }

        if (CoinPrefab.GetComponent<CoinScript>() != null)
        {
            mCoinHeight = CoinPrefab.GetComponent<CoinScript>().CoinMeshRenderer.bounds.size.y;
        } 
        else
        {
            Debug.Log("Told to spawn a coin type with no renderer! Not doing that!", this);
            this.enabled = false;
            return;
        }

        mStartDelay = Random.Range(StartDelayMin, StartDelayMax);
        mSpawnAmount = Random.Range(MinCoinAmount, MaxCoinAmount);
        mPosition = transform.position;
	}
	
	// Update is called once per frame
	private void Update () 
    {
	    if (mCurrentStartDelay < mStartDelay)
        {
            mCurrentStartDelay += Time.deltaTime;
            return;
        }

        if (mCurrentSpawnDelay < SpawnDelay)
        {
            mCurrentSpawnDelay += Time.deltaTime;
            return;
        }

        SpawnCoin();
        mCurrentSpawnDelay = 0f;
	}

    private void SpawnCoin()
    {
        Vector3 position = mPosition;

        RaycastHit hit;
        Ray testRay = new Ray(mPosition + (Vector3.up * 100), Vector3.down);

        int layerMask = 1 << LayerMask.NameToLayer("CoinLayer");

        if (Physics.Raycast(testRay, out hit, 1000f, layerMask))
        {
            position = hit.point +  new Vector3(0f, mCoinHeight * 0.5f, 0f);
        }

        Vector3 positionOffset = Vector3.zero;

        positionOffset += Vector3.right * Random.Range(-MaxOffset, MaxOffset);
        positionOffset += Vector3.forward * Random.Range(-MaxOffset, MaxOffset);

        position += positionOffset;

        Vector3 rot = new Vector3(0f, Random.Range(-MaxRotation, MaxRotation), 0f);

        GameObject newCoin = (GameObject)GameObject.Instantiate(CoinPrefab, position, Quaternion.Euler(rot));

        newCoin.transform.parent = transform;

        mCurrentSpawnCount++;

        if (mCurrentSpawnCount > mSpawnAmount)
        {
            this.enabled = false;
            //gameObject.SetActive(false);
        }
    }
}
