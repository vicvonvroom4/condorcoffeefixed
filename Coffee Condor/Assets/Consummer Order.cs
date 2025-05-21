using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumerOrder : MonoBehaviour
{
    private int maxNumber = 1000000;
    public int numberOfSections = 4;
    void Start()
    {
        RNG();
    }
    public int GetRandomNumber()
    {
        return Random.Range(1, maxNumber + 1);
    }
    public void RNG()
    {
        if (numberOfSections <= 0 || maxNumber < 1)
        {
            Debug.LogWarning("Invalid configuration");
            return;
        }
        int randomNumber = GetRandomNumber();
        int sectionSize = Mathf.CeilToInt((float)maxNumber / numberOfSections);
        int section = Mathf.Clamp(((randomNumber - 1) / sectionSize) + 1, 1, numberOfSections);
        Debug.Log($"Random number: {randomNumber} is in section {section} (out of {numberOfSections})");
    }
}
