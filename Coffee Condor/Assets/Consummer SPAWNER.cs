using System.Collections;
using UnityEngine;

public class ConsummerSPAWNER : MonoBehaviour
{
    public GameObject consumerPrefab;
    public Transform spawnPoint;
    public int spawnCount = 0;
    public int maxSpawnCount = 0;
    public DAYTime dayTimeManager; // Assign in Inspector
    public bool DAY = true;
    private void Start()
    {
        FindMaxSpawnCount();
        StartCoroutine(SpawnLoop());
    }
    private void FixedUpdate()
    {
        if (spawnCount < maxSpawnCount)
        {
            DAY = false;
        }
    }
    IEnumerator SpawnLoop()
    {
        while (spawnCount < maxSpawnCount && DAY)
        {
            spawnCount++;
            SpawnConsumer();
            float waitTime = Random.Range(5f, 10f);
            yield return new WaitForSeconds(waitTime);
            Debug.Log("Spawned consumer: " + spawnCount);
        }

        // Spawn limit reached
        if (dayTimeManager != null)
        {
            dayTimeManager.StartDayEndSequence();
        }
    }

    public void ResetSpawner()
    {
        spawnCount = 0;
        FindMaxSpawnCount();

        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(SpawnLoop());
        }
    }

    void FindMaxSpawnCount()
    {
        maxSpawnCount = Random.Range(20, 31);
        Debug.Log("New max spawn count: " + maxSpawnCount);
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
