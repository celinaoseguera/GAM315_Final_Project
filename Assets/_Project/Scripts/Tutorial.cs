using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tutorial : MonoBehaviour
{
    [SerializeField] GameObject[] stepToShow;
    [SerializeField] GameObject background;
    [SerializeField] TextMeshProUGUI[] numsCountdown;
    private float timer;
    private float stepTimer;
    private float countTimer;

    void Awake()
    {
        Time.timeScale = 0f;
        background.SetActive(true);
        stepTimer = 0f;
        countTimer = 0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        //setting false on onset
        foreach (GameObject step in stepToShow)
        {
            step.SetActive(false);

        }
        //setting all active in order
        foreach (GameObject step in stepToShow)
        {

            StartCoroutine(DelayNextStep(3f+stepTimer, step));
            stepTimer += 3f;
        }
        

    }

    private IEnumerator DelayNextStep(float waitTime, GameObject step)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        step.SetActive(true);

        if (step == stepToShow[5])
        {
            Debug.Log("made it to countdown");
            foreach(TextMeshProUGUI count in numsCountdown)
            {
                StartCoroutine(DelayCounter(1f, count));
                countTimer += 1f;
            }
            step.SetActive(false);
        }
        else
        {
            StartCoroutine(DeleteStep(3f, step));
        }
    }

    private IEnumerator DeleteStep(float waitTime, GameObject step)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        step.SetActive(false);
    }

    private IEnumerator DelayCounter(float waitTime, TextMeshProUGUI count)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        count.enabled = true;
        StartCoroutine(DeleteCounter(1f, count));


    }

    private IEnumerator DeleteCounter(float waitTime, TextMeshProUGUI count)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        count.enabled = false;

    }
}
