using System.Collections;
using UnityEngine;

public class DAYTime : MonoBehaviour
{
    public GameObject resultCanvas; // Assign in Inspector
    public ConsummerSPAWNER spawner; // Reference to the spawner script

    public void StartDayEndSequence()
    {
        StartCoroutine(EndDayRoutine());
    }

    private IEnumerator EndDayRoutine()
    {
        Debug.Log("Day ending. Waiting 30 seconds...");
        yield return new WaitForSeconds(30f);

        // Show results canvas
        if (resultCanvas != null)
        {
            resultCanvas.SetActive(true);
        }

        // Reset the spawner
        if (spawner != null)
        {
            spawner.ResetSpawner();
        }

        ConsumerOrder.ResetSectionStats();

        Debug.Log("Day reset.");
    }
}
