using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsummerMovemnet : MonoBehaviour
{
    private Transform orderTarget;
    private Transform eatTarget;
    private Transform spawnPoint;
    private Transform leavePoint;

    void Start()
    {
        // Validate MoneyManager and array contents
        if (MoneyManager.main == null ||
            MoneyManager.main.OrderPoint.Length == 0 ||
            MoneyManager.main.EatPoint.Length == 0)
        {
            Debug.LogError("MoneyManager or required arrays are not set correctly.");
            return;
        }

        // Assign spawn and leave points
        spawnPoint = MoneyManager.main.SpawnPoint;
        leavePoint = MoneyManager.main.LeavePoint;

        // Select random destinations
        SelectRandomDestinations();

        Debug.Log("Spawn at: " + spawnPoint.name);
        Debug.Log("Order at: " + orderTarget.name);
        Debug.Log("Eat at: " + eatTarget.name);
        Debug.Log("Leave at: " + leavePoint.name);
    }

    void SelectRandomDestinations()
    {
        Transform[] orderPoints = MoneyManager.main.OrderPoint;
        Transform[] eatPoints = MoneyManager.main.EatPoint;

        orderTarget = orderPoints[Random.Range(0, orderPoints.Length)];
        eatTarget = eatPoints[Random.Range(0, eatPoints.Length)];
    }
}
