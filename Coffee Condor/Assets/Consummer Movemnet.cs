using System.Collections;
using UnityEngine;

public class ConsummerMovemnet : MonoBehaviour
{
    private Transform orderTarget;
    private Transform eatTarget;
    private Transform spawnPoint;
    private Transform leavePoint;

    public float moveSpeed = 2f; // Speed of movement

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

        // Start movement sequence
        StartCoroutine(MoveSequence());
    }

    void SelectRandomDestinations()
    {
        Transform[] orderPoints = MoneyManager.main.OrderPoint;
        Transform[] eatPoints = MoneyManager.main.EatPoint;

        orderTarget = orderPoints[Random.Range(0, orderPoints.Length)];
        eatTarget = eatPoints[Random.Range(0, eatPoints.Length)];
    }

    IEnumerator MoveSequence()
    {
        // Move to Spawn (initial repositioning)
        transform.position = spawnPoint.position;
        yield return new WaitForSeconds(0.5f); // Optional pause at spawn

        // Move to Order point
        yield return StartCoroutine(MoveToTarget(orderTarget));

        // Pause to simulate ordering
        yield return new WaitForSeconds(1f);

        // Move to Eat point
        yield return StartCoroutine(MoveToTarget(eatTarget));

        // Pause to simulate eating
        yield return new WaitForSeconds(2f);

        // Move to Leave point
        yield return StartCoroutine(MoveToTarget(leavePoint));

        // Final behavior (e.g., destroy object)
        Debug.Log("Consumer finished sequence.");
        Destroy(gameObject); // Optional: remove NPC from scene
    }

    IEnumerator MoveToTarget(Transform target)
    {
        while (Vector3.Distance(transform.position, target.position) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
