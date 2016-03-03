using UnityEngine;
using System.Collections;

public class CoinSpawner : MonoBehaviour 
{
    public GameObject CoinPrefab;
    public float MaxAngularVelocity = 10f;
    //public Vector3 AngularVelocityMin = Vector3.zero;
    //public Vector3 AngularVelocityMax = Vector3.zero;
    public Vector3 RotationMin = Vector3.zero;
    public Vector3 RotationMax = Vector3.zero;
    public float SpawnRadius = 10f;
    public float SpawnHeight = 2f;
    public int InitialSpawnAmount = 10;
    public int AmountPerManualSpawn = 1;

	private void Start()
	{
        SpawnCoins(InitialSpawnAmount);
	}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            SpawnCoins(AmountPerManualSpawn);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ClearCoins();
        }
    }

    private void SpawnCoins(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Vector3 location = transform.position;

            location.x += Random.Range(-SpawnRadius, SpawnRadius);
            location.z += Random.Range(-SpawnRadius, SpawnRadius);
            location.y += Random.Range(-SpawnHeight, SpawnHeight);

            SpawnCoin(location);
        }
    }

    private void SpawnCoin(Vector3 location)
    {
        GameObject coin = (GameObject)GameObject.Instantiate(CoinPrefab, location, Quaternion.identity);

        Vector3 eulerRotation = Vector3.zero;

        eulerRotation.x = Random.Range(RotationMin.x, RotationMax.x);
        eulerRotation.y = Random.Range(RotationMin.y, RotationMax.y);
        eulerRotation.z = Random.Range(RotationMin.z, RotationMax.z);

        coin.transform.rotation = Quaternion.Euler(eulerRotation);

        Vector3 angularVelocity = Vector3.zero;
        Rigidbody coinRigidbody = coin.GetComponent<Rigidbody>();


        coinRigidbody.maxAngularVelocity = MaxAngularVelocity;

        angularVelocity.x = Random.Range(-MaxAngularVelocity, MaxAngularVelocity);
        angularVelocity.y = Random.Range(-MaxAngularVelocity, MaxAngularVelocity);
        angularVelocity.z = Random.Range(-MaxAngularVelocity, MaxAngularVelocity);

        coinRigidbody.angularVelocity = angularVelocity;

        coin.transform.parent = transform;
    }

    private void ClearCoins()
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, SpawnRadius);
    }
}
