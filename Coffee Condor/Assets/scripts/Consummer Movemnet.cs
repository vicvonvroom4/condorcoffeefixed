using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsummerMovemnet : MonoBehaviour
{
    private Transform orderTarget;
    private Transform eatTarget;
    private Transform spawnPoint;
    private Transform leavePoint;

    public float moveSpeed = 2f;

    private ConsumerOrder orderComponent;
    public Transform CurrentPoint { get; private set; }

    void Start()
    {
        if (MoneyManager.main == null ||
            MoneyManager.main.OrderPoint.Length == 0 ||
            MoneyManager.main.EatPoint.Length == 0)
        {
            Debug.LogError("MoneyManager or required arrays are not set correctly.");
            return;
        }

        spawnPoint = MoneyManager.main.SpawnPoint;
        leavePoint = MoneyManager.main.LeavePoint;

        SelectRandomDestinations();

        orderComponent = GetComponent<ConsumerOrder>();
        if (orderComponent == null)
        {
            Debug.LogError("Missing ConsumerOrder component on NPC.");
            return;
        }

        StartCoroutine(MoveSequence());
    }

    void SelectRandomDestinations()
    {
        Transform[] orderPoints = MoneyManager.main.OrderPoint;
        Transform[] eatPoints = MoneyManager.main.EatPoint;

        // Select random unoccupied order point
        orderTarget = GetAvailablePoint(orderPoints);
        if (orderTarget == null)
        {
            Debug.LogWarning("No available OrderPoint.");
        }

        // Select random unoccupied eat point
        eatTarget = GetAvailablePoint(eatPoints);
        if (eatTarget == null)
        {
            Debug.LogWarning("No available EatPoint.");
        }
    }


    IEnumerator MoveSequence()
    {
        // Optional: move to spawnPoint if not already there
        if (Vector3.Distance(transform.position, spawnPoint.position) > 0.1f)
        {
            yield return MoveToTarget(spawnPoint);
        }

        // Wait until an order point becomes free (fallback)
        while (orderTarget == null)
        {
            SelectRandomDestinations();
            yield return new WaitForSeconds(0.5f);
        }

        // Move to order point and trigger RNG after arrival
        yield return MoveToTarget(orderTarget);

        // Start the ordering coroutine
        StartCoroutine(orderComponent.WaitForOrder());

        // Wait until order is received
        while (!orderComponent.gotOrder)
        {
            yield return null;
        }

        // Move to eat point
        yield return MoveToTarget(eatTarget);

        // Wait a few seconds to simulate eating
        yield return new WaitForSeconds(3f);

        // Move to leave point
        yield return MoveToTarget(leavePoint);

        // Self-destruct
        Destroy(gameObject);
    }


    IEnumerator MoveToTarget(Transform target, bool callRNG = false)
    {
        // Clear old point if any
        if (CurrentPoint != null)
        {
            ConsumerPoint oldPoint = CurrentPoint.GetComponent<ConsumerPoint>();
            if (oldPoint != null)
            {
                oldPoint.ClearConsumer();
            }
        }

        // Move to new point
        while (Vector3.Distance(transform.position, target.position) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
            yield return null;
        }

        CurrentPoint = target;

        ConsumerPoint newPoint = target.GetComponent<ConsumerPoint>();
        if (newPoint != null && !newPoint.IsOccupied)
        {
            newPoint.TryClaim(this);
        }

        // Trigger RNG here if needed
        if (callRNG && orderComponent != null)
        {
            orderComponent.BalancedRNG();
        }
    }

    Transform GetAvailablePoint(Transform[] points)
    {
        List<Transform> shuffled = new List<Transform>(points);

        // Shuffle list to avoid predictable order
        for (int i = 0; i < shuffled.Count; i++)
        {
            int randIndex = Random.Range(i, shuffled.Count);
            var temp = shuffled[i];
            shuffled[i] = shuffled[randIndex];
            shuffled[randIndex] = temp;
        }

        foreach (Transform t in shuffled)
        {
            ConsumerPoint ap = t.GetComponent<ConsumerPoint>();
            if (ap != null && ap.TryClaim(this))
            {
                return t;
            }
        }

        return null; // No available point
    }



}