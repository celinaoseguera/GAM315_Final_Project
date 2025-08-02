using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] GameObject[] stepToShow;
    [SerializeField] GameObject background;
    private float timer;
    private int stepCounter;

    // Start is called before the first frame update
    void Awake()
    {
        Time.timeScale = 0f;
        background.SetActive(true);
        foreach (GameObject step in stepToShow)
        {
            step.SetActive(false);
            StartCoroutine(DelayNextStep(2f, stepCounter));
            stepCounter++;
        }
        

    }

    private IEnumerator DelayNextStep(float waitTime, int stepNum)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        stepToShow[stepNum].SetActive(true);
    }

}
