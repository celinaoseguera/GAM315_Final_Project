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

    //Tutorial items to use in ALL steps
    [SerializeField] GameObject farmerTutorial;
    private Vector2 farmerPos;

    // Tutorial items to show step1
    [SerializeField] GameObject soilPlotTutorial;
    [SerializeField] GameObject cropToSpawnTutorial;
    [SerializeField] SpriteChanger spriteChangerTutorial;
    [SerializeField] GameObject[] stepOneInstruct;
    private GameObject spawnedCrop;


    // Tutorial items to show step2/3
    [SerializeField] GameObject npcTutorial;
    [SerializeField] GameObject requestToSpawnTutorial;
    [SerializeField] GameObject failureToSpawnTutorial;
    [SerializeField] GameObject completeToSpawnTutorial;
    [SerializeField] GameObject stepTwoInstruct;
    [SerializeField] TMP_Text MoneyToChange;
    private GameObject requestSpawned;
    private GameObject failureSpawned;
    private GameObject completeSpawned;
    private Vector2 npcPos;
    private Vector2 npcPosOffsetY;
    private Vector2 npcPosOffsetXY;

    // Tutorial items to show step4
    [SerializeField] GameObject shopTutorial;
    [SerializeField] GameObject stepFourInstruct;
    [SerializeField] GameObject seedToSpawnTutorial;
    [SerializeField] GameObject availableBox;
    [SerializeField] TMP_Text availableSeedsNumTutorial;
    [SerializeField] TMP_Text availableSeedsTxtTutorial;
    private GameObject seedSpawnedTutorial;
    private Vector2 shopPos;

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
        npcPos = npcTutorial.transform.position;
        npcPosOffsetY = new Vector2(npcPos.x, npcPos.y + 1.3f);
        npcPosOffsetXY = new Vector2(npcPos.x + .9f, npcPos.y + 1.3f);
        shopPos = shopTutorial.transform.position;
        //setting false on onset
        foreach (GameObject step in stepToShow)
        {
            step.SetActive(false);
        }
        // step 0 activation
        StartCoroutine(DelayNextStep(2f, stepToShow[0], spacebarHelpText, farmerTutorial));


    }
    // steps 1- end functionality code block
    // separated by if/case statements
    private void NextStep(object sender, EventArgs e)
    {
        //delete prev step before proceeded to next step (current step now)
        currentStep.SetActive(false);
        currentStepIndex++;
        currentStep = stepToShow[currentStepIndex];
        currentStep.SetActive(true);

        // if that step is step [1]

        if (currentStep == stepToShow[1])
        {
            // delete prev items/layers from step[0]
            foreach (GameObject step in stepOneInstruct)
            {
                step.SetActive(false);
            }
            Destroy(spawnedCrop);
            cropToSpawnTutorial.GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("Plants");
            soilPlotTutorial.GetComponent<SpriteRenderer>().color = new Color(0f, 0.5019608f, 0.5019608f, 0f);

            farmerTutorial.transform.position = new Vector2(1.23f, 2.65f);
            npcTutorial.GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("Tutorial");
            requestSpawned = Instantiate(requestToSpawnTutorial, npcPosOffsetY, Quaternion.identity);
            completeSpawned = Instantiate(completeToSpawnTutorial, npcPosOffsetXY, Quaternion.identity);
            stepTwoInstruct.SetActive(true);
        }
        if (currentStep == stepToShow[2])
        {
            // delete prev items/layers
            Destroy(completeSpawned);

            failureSpawned = Instantiate(failureToSpawnTutorial, npcPosOffsetXY, Quaternion.identity);
        }

        if (currentStep == stepToShow[3])
        {
            // delete prev items/layers
            Destroy(failureSpawned);

            completeSpawned = Instantiate(completeToSpawnTutorial, npcPosOffsetXY, Quaternion.identity);
            MoneyToChange.text = "1";

        }

        if (currentStep == stepToShow[4])
        {
            // delete prev items/layers
            Destroy(completeSpawned);
            Destroy(requestSpawned);
            MoneyToChange.text = "0";
            npcTutorial.GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("Foreground");

            stepFourInstruct.SetActive(true);
            farmerTutorial.transform.position = new Vector2(-2.01f, 0.57f);
            shopTutorial.GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("Tutorial");
            seedSpawnedTutorial = Instantiate(seedToSpawnTutorial, new Vector2(shopPos.x + .4f, shopPos.y + 1.9f), Quaternion.identity);
            availableBox.SetActive(true);
            availableSeedsNumTutorial.text = "4";
            availableSeedsTxtTutorial.text = "available";


        }

        // if that step is last step [5]
        if (currentStep == stepToShow[5])
        {
            // delete prev items/layers
            stepFourInstruct.SetActive(false);
            farmerTutorial.transform.position = new Vector2(-0.046742f, -0.0077903f);
            farmerTutorial.GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("Foreground");
            shopTutorial.GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("Foreground");
            Destroy(seedSpawnedTutorial);
            availableBox.SetActive(false);
            availableSeedsNumTutorial.text = "";
            availableSeedsTxtTutorial.text = "";

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
        // setting up activations for step 1
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
        cropToSpawnTutorial.GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("Tutorial");
        cropToSpawnTutorial.GetComponent<SpriteRenderer>().sprite = spriteChangerTutorial.cropSprites[0];
        soilPlotTutorial.GetComponent<SpriteRenderer>().color = new Color(0f, 0.5019608f, 0.5019608f, .9f);
        spawnedCrop = Instantiate(cropToSpawnTutorial, soilPlotTutorial.transform.position, Quaternion.identity);
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
        if (spawnedCrop != null)
        {
            spawnedCrop.GetComponent<SpriteRenderer>().sprite = spriteChangerTutorial.cropSprites[0];
            StartCoroutine(DelayCrop1(2f, spawnedCrop));
        }
    }

    private IEnumerator DelayCrop1(float waitTime, GameObject spawnedCrop)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        if (spawnedCrop != null)
        {
            spawnedCrop.GetComponent<SpriteRenderer>().sprite = spriteChangerTutorial.cropSprites[1];
            StartCoroutine(DelayCrop2(2f, spawnedCrop));
        }
    }

    private IEnumerator DelayCrop2(float waitTime, GameObject spawnedCrop)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        if (spawnedCrop != null)
        {
            spawnedCrop.GetComponent<SpriteRenderer>().sprite = spriteChangerTutorial.cropSprites[2];
            StartCoroutine(DelayCrop0(2f, spawnedCrop));
        }
    }


}
