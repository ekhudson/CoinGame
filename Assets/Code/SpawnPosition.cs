using UnityEngine;

public class SpawnPosition : MonoBehaviour
{
    private void Start()
    {
        PogManager.Instance.SetSpawnPosition(transform);
    }
}
