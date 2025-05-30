using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumerOrder : MonoBehaviour
{
    private int maxNumber = 1000000;
    public int numberOfSections = 4;
    public bool gotOrder = false;

    public GameObject[] sectionPrefabs;
    public Transform spawnOffset;

    private GameObject activeSectionVisual; // store reference to disable later
    private static int[] sectionCounts;
    private static int lastPicked = -1;
    private static int streakCount = 0;
    public bool orderProcessed = false;
    public bool acceptingOrder = false;
    public bool eating = false;
    private void Awake()
    {
        if (sectionCounts == null || sectionCounts.Length != numberOfSections)
        {
            sectionCounts = new int[numberOfSections];
        }
    }
    public void Update()
    {
        if (acceptingOrder && gotOrder && !orderProcessed)
        {
            CompleteOrder();
            orderProcessed = true;
        }
    }

    public IEnumerator WaitForOrder()
    {
        yield return new WaitForSeconds(2f);

        int selectedSection = BalancedRNG();
        SpawnSectionVisual(selectedSection);
        acceptingOrder = true;
    }


    void SpawnSectionVisual(int section)
    {
        if (sectionPrefabs == null || sectionPrefabs.Length < section)
        {
            Debug.LogWarning("Section prefab missing or index out of range.");
            return;
        }

        Vector3 spawnOffset = new Vector3(0, 1.5f, 0); // adjust height above consumer
        GameObject prefab = sectionPrefabs[section - 1];

        // Instantiate and parent to this consumer
        activeSectionVisual = Instantiate(prefab, transform.position + spawnOffset, Quaternion.identity, this.transform);
    }

    public int BalancedRNG()
    {
        int sectionSize = Mathf.CeilToInt((float)maxNumber / numberOfSections);

        int selected = -1;
        int attempts = 0;
        do
        {
            int rand = Random.Range(1, maxNumber + 1);
            selected = Mathf.Clamp(((rand - 1) / sectionSize), 0, numberOfSections - 1);

            attempts++;

            // Avoid streak
            if (selected == lastPicked)
            {
                streakCount++;
            }
            else
            {
                streakCount = 1;
            }

            // If the streak exceeds 3, force pick a different one
            if (streakCount > 3)
            {
                selected = PickLeastUsedSection(exclude: lastPicked);
                break;
            }

            // Break if reasonably balanced after a few tries
            if (attempts > 5)
            {
                break;
            }

        } while (!IsBalancedPick(selected));

        sectionCounts[selected]++;
        lastPicked = selected;

        Debug.Log($"Selected section {selected + 1} (Count: {sectionCounts[selected]})");

        return selected + 1;
    }

    bool IsBalancedPick(int index)
    {
        int current = sectionCounts[index];
        int max = 0, min = int.MaxValue;

        foreach (int count in sectionCounts)
        {
            if (count > max) max = count;
            if (count < min) min = count;
        }

        // If this index is more than 2 over the min, it's over-picked
        if (current > min + 2) return false;

        return true;
    }

    int PickLeastUsedSection(int exclude = -1)
    {
        int min = int.MaxValue;
        int pick = 0;

        for (int i = 0; i < sectionCounts.Length; i++)
        {
            if (i == exclude) continue;
            if (sectionCounts[i] < min)
            {
                min = sectionCounts[i];
                pick = i;
            }
        }

        return pick;
    }
    public static void ResetSectionStats()
    {
        if (sectionCounts != null)
        {
            for (int i = 0; i < sectionCounts.Length; i++)
            {
                sectionCounts[i] = 0;
            }
        }

        lastPicked = -1;
        streakCount = 0;
    }
    public void CompleteOrder()
    {
        gotOrder = true;
        eating = true;
        // Award money when the order is completed
        if (MoneyManager.main != null)
        {
            MoneyManager.main.IncreaseCurrency(100);
        }

        // Destroy or disable the section visual
        if (activeSectionVisual != null)
        {
            Destroy(activeSectionVisual);
        }
    }


}
