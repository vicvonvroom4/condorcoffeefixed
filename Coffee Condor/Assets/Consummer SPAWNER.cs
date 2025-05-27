using System.Collections;
using UnityEngine;

public class ConsummerSPAWNER : MonoBehaviour
{
    public GameObject consumerPrefab; // The asset to spawn
    public Transform spawnPoint;      // The location to spawn at
    public int spawnCount = 0;
    public int maxSpawnCount = 0;
    private void Start()
    {
        FindMaxSpawnCount(); // Set maxSpawnCount randomly
        StartCoroutine(SpawnLoop());
    }



    IEnumerator SpawnLoop()
    {
        while (true)
        {
            if (spawnCount < maxSpawnCount - 1)
            {
                spawnCount++;
                SpawnConsumer();
                float waitTime = Random.Range(5f, 10f);
                yield return new WaitForSeconds(waitTime);
                Debug.Log(spawnCount);
            }
        }
    }
    void FindMaxSpawnCount()
    {
        maxSpawnCount = Random.Range(20, 31); // 31 is exclusive, so this gives a value between 20 and 30
    }

    void SpawnConsumer()
    {
        if (consumerPrefab != null && spawnPoint != null)
        {
            Instantiate(consumerPrefab, spawnPoint.position, spawnPoint.rotation);
        }
        else
        {
            Debug.LogWarning("Consumer prefab or spawn point not set.");
        }
    }
}
