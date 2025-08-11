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
    [SerializeField] GameObject directionals;
    private GameObject currentStep;
    private int currentStepIndex;
    private float delayCountStep;
    private bool nextStepSubscribed;

    // Tutorial items to show step1

    [SerializeField] GameObject farmerTutorial;
    [SerializeField] GameObject soilPlot;
    [SerializeField] GameObject cropToSpawn;
    [SerializeField] SpriteChanger spriteChanger;
    [SerializeField] GameObject[] stepOneInstruct;
    private GameObject spawnedCrop;
    private Vector2 farmerPos;


    //public event EventHandler<OnCropTutArgs> OnCropTut;
    //public class OnCropTutArgs : EventArgs
    //{
        //public Vector2 soilPlotPos;
    //}

    // Start is called before the first frame update
    void Start()
    {

        Time.timeScale = 0f;
        background.SetActive(true);
        delayCountStep = 0f;
        currentStep = stepToShow[0];
        currentStepIndex = 0;
        nextStepSubscribed = false;
        directionals.SetActive(false);
        farmerPos = farmerTutorial.transform.position;
        //setting false on onset
        foreach (GameObject step in stepToShow)
        {
            step.SetActive(false);
        }
        // set step 0 as active (with 2 second delay)
        StartCoroutine(DelayNextStep(2f, stepToShow[0], spacebarHelpText, farmerTutorial));


    }

    private void NextStep(object sender, EventArgs e)
    {
        //delete prev step before proceeded to next step (current step now)
        currentStep.SetActive(false);
        currentStepIndex++;
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

    private IEnumerator DelayNextStep(float waitTime, GameObject step, GameObject spacebar, GameObject farmerTutorial)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        if (nextStepSubscribed == false)
        {
            inputPublisher.OnSpacePressed += NextStep;
            nextStepSubscribed = true;
        }
        foreach(GameObject stepInst in stepOneInstruct)
        {
            stepInst.SetActive(true);
        }
        step.SetActive(true);
        spacebar.SetActive(true);
        farmerTutorial.GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("Tutorial");
        farmerTutorial.transform.position = new Vector2(2.26f, -1.85f);
        cropToSpawn.GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("Tutorial");
        cropToSpawn.GetComponent<SpriteRenderer>().sprite = spriteChanger.cropSprites[0];
        soilPlot.GetComponent<SpriteRenderer>().color = new Color(0f, 0.5019608f, 0.5019608f, .9f);
        spawnedCrop = Instantiate(cropToSpawn, soilPlot.transform.position, Quaternion.identity);
        StartCoroutine(DelayCrop0(0f, spawnedCrop));

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
            directionals.SetActive(true);
            Time.timeScale = 1f;

        }

    }

    private IEnumerator DelayCrop0(float waitTime, GameObject spawnedCrop)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        spawnedCrop.GetComponent<SpriteRenderer>().sprite = spriteChanger.cropSprites[0];
        StartCoroutine(DelayCrop1(2f, spawnedCrop));
    }

    private IEnumerator DelayCrop1(float waitTime, GameObject spawnedCrop)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        spawnedCrop.GetComponent<SpriteRenderer>().sprite = spriteChanger.cropSprites[1];
        StartCoroutine(DelayCrop2(2f, spawnedCrop));
    }

    private IEnumerator DelayCrop2(float waitTime, GameObject spawnedCrop)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        soilPlot.GetComponent<SpriteRenderer>().color = new Color(0f, 0.5019608f, 0.5019608f, 0f);
        spawnedCrop.GetComponent<SpriteRenderer>().sprite = spriteChanger.cropSprites[2];
        StartCoroutine(DelayCrop0(2f, spawnedCrop));
    }

    void Update()
    {
        // take away prev instructional items
        if (currentStep != stepToShow[0])
        {
            foreach (GameObject step in stepOneInstruct)
            {
                step.SetActive(false);
            }
            Destroy(spawnedCrop);
        }
    }

}
