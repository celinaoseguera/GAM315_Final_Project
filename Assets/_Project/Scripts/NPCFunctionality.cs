using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static InputPublisher;


public class NPCFunctionality : MonoBehaviour
{
    [SerializeField] InputPublisher inputPublisher;
    [SerializeField] SpriteRenderer spriteRenderer;
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
        // will put qualifier for if player has in inventory later...
        Debug.Log("request completed!");
        requestCompleted = true;
    }

    void FailedRequest()

    {
        requestFailed = true;
        failedNum++;
        Debug.Log("request failed!");
        spriteRenderer.color = new Color(1f, 0f, 0f, 1f);
        requestRaised = false;
    }

    private IEnumerator RaiseRequest(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        //spriteRenderer.color = new Color(0f, 1f, 0f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(RaiseRequest(3f));
        timer += Time.deltaTime;

        if (timer > 10 && requestCompleted == false)
        {
            // fail state 1/3
            FailedRequest();
            timer = 0;
        }


        if (failedNum == 3)
        {
            Debug.Log("GAME OVER");
            spriteRenderer.color = new Color(0f, 1f, 0f, 1f);
            Time.timeScale = 0f;
        }
            
        }
    
}
    
