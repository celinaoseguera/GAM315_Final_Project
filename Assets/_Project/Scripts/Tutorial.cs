using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tutorial : MonoBehaviour
{
    [SerializeField] GameObject[] stepToShow;
    [SerializeField] GameObject background;
    private TMP_Text tmpComponent;
    private float timer;
    private int stepCounter;
    private float stepTimer;

    void Awake()
    {
        Time.timeScale = 0f;
        background.SetActive(true);
        stepCounter = 0;
        stepTimer = 0f;
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
            StartCoroutine(DelayNextStep(2f+stepTimer, step,stepCounter));
            stepCounter++;
            Debug.Log(stepCounter);
            stepTimer += 3f;
        }
        

    }

    private IEnumerator DelayNextStep(float waitTime, GameObject step, int stepNum)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        step.SetActive(true);
        tmpComponent = GetComponentInChildren<TextMeshProUGUI>();
        tmpComponent.enabled = true;
        StartCoroutine(DeleteStep(3f, step));
    }

    private IEnumerator DeleteStep(float waitTime, GameObject step)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        step.SetActive(false);
    }
}
