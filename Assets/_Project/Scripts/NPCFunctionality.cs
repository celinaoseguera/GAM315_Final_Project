using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static InputPublisher;


public class NPCFunctionality : MonoBehaviour
{
    [SerializeField] InputPublisher inputPublisher;
    [SerializeField] SpriteRenderer request;
    [SerializeField] SpriteRenderer failed;
    [SerializeField] SpriteRenderer completed;
    private float timer;
    private float delayedTimer;
    private float start;
    private bool requestRaised;
    private bool requestCompleted;
    private bool requestFailed;
    private int failedNum;
    private const string PLAYER_TAG = "Player";

    // Start is called before the first frame update
    void Start()
    {
        requestRaised = false;
        requestCompleted = false;
        requestFailed = false;
        failedNum = 0;
        timer = 0;
        delayedTimer = 0;
        start = 0;
        request.color = new Color(0f, 0f, 1f, 0f); 
        failed.color = new Color(0f, 0f, 1f, 0f);
        completed.color = new Color(0f, 0f, 1f, 0f);

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
    // CELINA FIX SO E PRESS IS BEING PROCESSED
    private void GiveCrop(object sender, EventArgs e)
    {
        // will put qualifier for if player has in inventory later...
        Debug.Log("request completed!");
        completed.color = new Color(0f, 1f, 0f, 1f);
        failed.color = new Color(0f, 0f, 1f, 0f);
        requestCompleted = true;
        StartCoroutine(DelayRequestFade(1f));
        StartCoroutine(DelayRequestCompletedFade(1f));

    }

    void FailedRequest()

    {
        failedNum++;
        Debug.Log("request failed!");
        failed.color = new Color(1f, 0f, 0f, 1f);
        StartCoroutine(DelayRequestFailedFade(1f));
        StartCoroutine(DelayRequestFade(1f));
    }

    void RaiseRequest()
    {
        request.color = new Color(0f, 0f, 1f, 1f);
        requestRaised = true;

    }

    private IEnumerator DelayRequestFade(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        request.color = new Color(0f, 0f, 1f, 0f);
    }

    private IEnumerator DelayRequestFailedFade(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        failed.color = new Color(1f, 0f, 0f, 0f);
    }

    private IEnumerator DelayRequestCompletedFade(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        completed.color = new Color(0f, 1f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        
        timer += Time.deltaTime;

        if (timer > 3 && timer < 4)
        {
            RaiseRequest();
        }

        if (timer > 10 && timer < 11 && requestCompleted == false)
        {
            FailedRequest();
            timer = 0;
        }

        if (requestCompleted == true)
        {
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
    
