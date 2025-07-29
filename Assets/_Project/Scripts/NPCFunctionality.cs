using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static InputPublisher;
using static PlayerInventory;


public class NPCFunctionality : MonoBehaviour
{
    [SerializeField] InputPublisher inputPublisher;
    [SerializeField] GameObject requestStateToSpawn;
    [SerializeField] PlayerInventory playerInventory;
    private float timer;
    private bool requestRaised;
    private bool requestCompleted;
    private int failedNum;
    private const string PLAYER_TAG = "Player";

    public event EventHandler OnCropGiven;

    // Start is called before the first frame update
    void Start()
    {
        requestRaised = false;
        requestCompleted = false;
        failedNum = 0;
        timer = 0;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(PLAYER_TAG))
        {
            inputPublisher.OnEPressed += GiveCrop;
        }

        return;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(PLAYER_TAG))
        {
            inputPublisher.OnEPressed -= GiveCrop;
        }

        return;
    }

    private void GiveCrop(object sender, EventArgs e)
    {
        if (requestRaised == true && playerInventory.wheatAmount >= 1)
        {
            // will put qualifier for if player has in inventory later...
            //completed.color = new Color(0f, 1f, 0f, 1f);
            //failed.color = new Color(0f, 0f, 1f, 0f);
            StartCoroutine(DelayRequestFade(1f));
            StartCoroutine(DelayRequestCompletedFade(1f));
            requestCompleted = true;
            Debug.Log("Completed");
            requestRaised = false;
            timer = 0;
            OnCropGiven?.Invoke(this, EventArgs.Empty);
        }

    }

    void FailedRequest()

    {
        Debug.Log("Failed");
        failedNum++;
        //failed.color = new Color(1f, 0f, 0f, 1f);
        StartCoroutine(DelayRequestFailedFade(1f));
        StartCoroutine(DelayRequestFade(1f));
        requestRaised = false;
    }

    void RaiseRequest()
    {
        //request.color = new Color(0f, 0f, 1f, 1f);
        requestCompleted = false;
        requestRaised = true;

    }

    private IEnumerator DelayRequestFade(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        //request.color = new Color(0f, 0f, 1f, 0f);
    }

    private IEnumerator DelayRequestFailedFade(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        //failed.color = new Color(1f, 0f, 0f, 0f);
    }

    private IEnumerator DelayRequestCompletedFade(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        //completed.color = new Color(0f, 1f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        // ADD IN COROUTINE THAT IS JUST AN EMPTY FUNCTION THAT STALL FOR RANDON 1-5 TIME TO ALLOW FOR 
        // VARIANCE IN REQUEST START AMONGST THREE NPCS AT THE START
        timer += Time.deltaTime;

        if (timer > 10 && timer < 11)
        {
            RaiseRequest();
        }

        if (timer > 20 && timer < 21 && requestCompleted == false && requestRaised == true)
        {
    
            FailedRequest();
            timer = 0;
        }

        if (failedNum == 3)
            // record into gameover scropt that takers script array of all instasnces of npcs
            // so when failedNum == 3, gameover happens
        {
            Debug.Log("GAME OVER");
            Time.timeScale = 0f;
        }
            
        }
    
}
    
