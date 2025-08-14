using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tutorial : MonoBehaviour
{
    [SerializeField] GameObject[] stepToShow;
    [SerializeField] InputPublisher inputPublisher;
    [SerializeField] GameObject background;
    [SerializeField] TextMeshProUGUI[] numsCountdown;
    [SerializeField] GameObject spacebarHelpText;
    [SerializeField] GameObject directionals;
    private GameObject currentStep;
    private int currentStepIndex = 0;
    private float delayCountStep = 0f;
    private bool nextStepSubscribed = false;

    //Tutorial items to use in ALL steps
    [SerializeField] GameObject farmerTutorial;
    private Vector2 farmerPosTutorial;

    // Tutorial items to show step1
    [SerializeField] GameObject soilPlotTutorial;
    [SerializeField] GameObject cropToSpawnTutorial;
    [SerializeField] SpriteChanger spriteChangerTutorial;
    [SerializeField] GameObject[] stepOneInstructTutorial;
    private GameObject spawnedCropTutorial;


    // Tutorial items to show step2/3
    [SerializeField] GameObject npcTutorial;
    [SerializeField] GameObject requestToSpawnTutorial;
    [SerializeField] GameObject failureToSpawnTutorial;
    [SerializeField] GameObject completeToSpawnTutorial;
    [SerializeField] GameObject stepTwoInstructTutorial;
    [SerializeField] TMP_Text MoneyToChangeTutorial;
    private GameObject requestSpawnedTutorial;
    private GameObject failureSpawnedTutorial;
    private GameObject completeSpawnedTutorial;
    private Vector2 npcPosTutorial;
    private Vector2 npcPosOffsetYTutorial;
    private Vector2 npcPosOffsetXYTutorial;

    // Tutorial items to show step4
    [SerializeField] GameObject shopTutorial;
    [SerializeField] GameObject stepFourInstruct;
    [SerializeField] GameObject seedToSpawnTutorial;
    [SerializeField] GameObject availableBoxTutorial;
    [SerializeField] TMP_Text availableSeedsNumTutorial;
    [SerializeField] TMP_Text availableSeedsTxtTutorial;
    private GameObject seedSpawnedTutorial;
    private Vector2 shopPosTutorial;

    void Start()
    {

        Time.timeScale = 0f;
        background.SetActive(true);
        currentStep = stepToShow[0];
        directionals.SetActive(false);
        farmerPosTutorial = farmerTutorial.transform.position;
        npcPosTutorial = npcTutorial.transform.position;
        npcPosOffsetYTutorial = new Vector2(npcPosTutorial.x, npcPosTutorial.y + 1.3f);
        npcPosOffsetXYTutorial = new Vector2(npcPosTutorial.x + .9f, npcPosTutorial.y + 1.3f);
        shopPosTutorial = shopTutorial.transform.position;

        //setting false on onset
        foreach (GameObject step in stepToShow)
        {
            step.SetActive(false);
        }

        // step[0] activation
        StartCoroutine(DelayNextStep(2f, stepToShow[0], spacebarHelpText, farmerTutorial));


    }
    // step[1]- end functionality code block
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
            foreach (GameObject step in stepOneInstructTutorial)
            {
                step.SetActive(false);
            }
            Destroy(spawnedCropTutorial);
            cropToSpawnTutorial.GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("Plants");
            soilPlotTutorial.GetComponent<SpriteRenderer>().color = new Color(0f, 0.5019608f, 0.5019608f, 0f);

            farmerTutorial.transform.position = new Vector2(1.23f, 2.65f);
            npcTutorial.GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("Tutorial");
            requestSpawnedTutorial = Instantiate(requestToSpawnTutorial, npcPosOffsetYTutorial, Quaternion.identity);
            completeSpawnedTutorial = Instantiate(completeToSpawnTutorial, npcPosOffsetXYTutorial, Quaternion.identity);
            stepTwoInstructTutorial.SetActive(true);
        }

        if (currentStep == stepToShow[2])
        {
            // delete prev items/layers
            Destroy(completeSpawnedTutorial);

            failureSpawnedTutorial = Instantiate(failureToSpawnTutorial, npcPosOffsetXYTutorial, Quaternion.identity);
        }

        if (currentStep == stepToShow[3])
        {
            // delete prev items/layers
            Destroy(failureSpawnedTutorial);

            completeSpawnedTutorial = Instantiate(completeToSpawnTutorial, npcPosOffsetXYTutorial, Quaternion.identity);
            MoneyToChangeTutorial.text = "1";

        }

        if (currentStep == stepToShow[4])
        {
            // delete prev items/layers
            Destroy(completeSpawnedTutorial);
            Destroy(requestSpawnedTutorial);
            MoneyToChangeTutorial.text = "0";
            npcTutorial.GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("Foreground");

            stepFourInstruct.SetActive(true);
            farmerTutorial.transform.position = new Vector2(-2.01f, 0.57f);
            shopTutorial.GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("Tutorial");
            seedSpawnedTutorial = Instantiate(seedToSpawnTutorial, new Vector2(shopPosTutorial.x + .4f, shopPosTutorial.y + 1.9f), Quaternion.identity);
            availableBoxTutorial.SetActive(true);
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
            availableBoxTutorial.SetActive(false);
            availableSeedsNumTutorial.text = "";
            availableSeedsTxtTutorial.text = "";

            inputPublisher.OnSpacePressed -= NextStep;
            spacebarHelpText.SetActive(false);
            currentStep.SetActive(true);

            // starting our 3,2,1 countdown 
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
    // for showing step [0], activated further up with a StartCoroutine
    private IEnumerator DelayNextStep(float waitTime, GameObject step, GameObject spacebar, GameObject farmerTutorial)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        // setting up activations for step 1
        if (nextStepSubscribed == false)
        {
            inputPublisher.OnSpacePressed += NextStep;
            nextStepSubscribed = true;
        }
        foreach(GameObject stepInst in stepOneInstructTutorial)
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
        spawnedCropTutorial = Instantiate(cropToSpawnTutorial, soilPlotTutorial.transform.position, Quaternion.identity);
        // starting the crop cycle of step[0]
        StartCoroutine(DelayCrop0(0f, spawnedCropTutorial));

    }

    // for showing the countdown in step[5]
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

    // for showing crop cycle in step[0]
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
