using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static InputPublisher;

public class Tutorial : MonoBehaviour
{
    [SerializeField] GameObject[] stepToShow;
    [SerializeField] InputPublisher inputPublisher;
    [SerializeField] GameObject background;
    [SerializeField] TextMeshProUGUI[] numsCountdown;
    [SerializeField] GameObject spacebarHelpText;
    private GameObject currentStep;
    private int currentStepIndex;
    private float timer;
    private float delayCountStep;
    private bool nextStepSubscribed;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0f;
        background.SetActive(true);
        delayCountStep = 0f;
        currentStep = stepToShow[0];
        currentStepIndex = 0;
        nextStepSubscribed = false;
        //setting false on onset
        foreach (GameObject step in stepToShow)
        {
            step.SetActive(false);
        }
        // set step 0 as active (with 2 second delay)
        StartCoroutine(DelayNextStep(2f, stepToShow[0]));
        StartCoroutine(DelayNextStep(2f, spacebarHelpText));


    }

    private void NextStep(object sender, EventArgs e)
    {
        //delete prev step before proceeded to next step (current step now)
        currentStep.SetActive(false);
        currentStepIndex++;
        Debug.Log(currentStepIndex);
        currentStep = stepToShow[currentStepIndex];
        currentStep.SetActive(true);
        // if that step is last step [5]
        if (currentStep == stepToShow[5])
        {
            inputPublisher.OnSpacePressed -= NextStep;
            spacebarHelpText.SetActive(false);
            currentStep.SetActive(true);

            foreach (TextMeshProUGUI count in numsCountdown)
            {
                count.enabled = false;
            }

            foreach (TextMeshProUGUI count in numsCountdown)
            {

                StartCoroutine(DelayCounter(1f + delayCountStep, count, currentStep));
                delayCountStep += 1f;

            }
        }
    }

    private IEnumerator DelayNextStep(float waitTime, GameObject step)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        if (nextStepSubscribed == false)
        {
            inputPublisher.OnSpacePressed += NextStep;
            nextStepSubscribed = true;
        }
        step.SetActive(true);
    }


    private IEnumerator DelayCounter(float waitTime, TextMeshProUGUI count, GameObject step)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        count.enabled = true;
        StartCoroutine(DeleteCounter(1f, count, step));
        


    }

    private IEnumerator DeleteCounter(float waitTime, TextMeshProUGUI count, GameObject step)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        count.enabled = false;
        if (count == numsCountdown[2])
        {
            step.SetActive(false);
            background.SetActive(false);
            Time.timeScale = 1f;
            
        }

    }
}
