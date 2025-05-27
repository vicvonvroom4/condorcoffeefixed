using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class makecoffee : MonoBehaviour

{
    public float countdownTime = 5f; // seconds 
    public GameObject product;  // assign product in the prospector 

    // Start is called before the first frame update
    void Start()
    {
        if (product == null)
            product.SetActive(false); //hide at first
    }

    // Update is called once per frame
    public void HasInteracted()
    {
        if (HasInteracted)
        {
            HasInteracted = true;
            StartCoroutine(StartCountdown());
        }
    }
    IEnumerator StartCountdown()
    {
        float timeLeft = countdownTime;

        while (timeLeft > 0)
        {
            if (countdownText != null)
                countdownText.text = Mathf.Ceil(timeLeft).ToString();

            yield return new WaitForSeconds(1f);
            timeLeft -= 1f;
        }

        if (countdownText != null)
            countdownText.text = "";

        if (product != null)
            product.SetActive(true);
    }


}
