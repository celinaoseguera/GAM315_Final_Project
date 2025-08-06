using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static InputPublisher;
using static PlayerInventory;


public class NPCFunctionality : MonoBehaviour
{
    //NPC requesters
    [SerializeField] InputPublisher inputPublisher;
    private bool requestRaised;
    private bool requestCompleted;
    private int failedNum;
    private Vector2 npcPos;
    private Vector2 npcPosOffsetY;
    private Vector2 npcPosOffsetXY;

    public event EventHandler OnCropGiven;

    //NPC states
    [SerializeField] GameObject requestToSpawn;
    [SerializeField] GameObject failureToSpawn;
    [SerializeField] GameObject completeToSpawn;
    private GameObject requestSpawned;
    private GameObject failureSpawned;
    private GameObject completeSpawned;
    private int spawnCountFlag;
    private float timer;
    private float randomDelay;

    public event EventHandler OnFailure;
    public event EventHandler OnCompleted;


    //Player
    [SerializeField] PlayerInventory playerInventory;
    private const string PLAYER_TAG = "Player";

    // Start is called before the first frame update
    void Start()
    {
        npcPos = this.transform.position;
        npcPosOffsetY = new Vector2 (npcPos.x, npcPos.y + 1.3f);
        npcPosOffsetXY = new Vector2(npcPos.x + .9f, npcPos.y + 1.3f);
        requestRaised = false;
        requestCompleted = false;
        spawnCountFlag = 0;
        failedNum = 0;
        timer = 0;
        randomDelay = 0f;

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
            completeSpawned = Instantiate(completeToSpawn, npcPosOffsetXY, Quaternion.identity);
            StartCoroutine(DelayRequestFade(2f));
            StartCoroutine(DelayRequestCompletedFade(2f));
            requestCompleted = true;
            requestRaised = false;
            timer = 0;
            spawnCountFlag = 0;
            OnCompleted?.Invoke(this, EventArgs.Empty);
            OnCropGiven?.Invoke(this, EventArgs.Empty);
        }

    }

    void FailedRequest()

    {
        failedNum++;
        Debug.Log(failedNum);
        failureSpawned = Instantiate(failureToSpawn, npcPosOffsetXY, Quaternion.identity);
        StartCoroutine(DelayRequestFailedFade(2f));
        StartCoroutine(DelayRequestFade(2f));
        requestRaised = false;
        OnFailure?.Invoke(this, EventArgs.Empty);
    }

    void RaiseRequest()
    {
        requestSpawned = Instantiate(requestToSpawn, npcPosOffsetY, Quaternion.identity);
        requestCompleted = false;
        requestRaised = true;

    }

    private IEnumerator DelayRequestFade(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(requestSpawned);
    }

    private IEnumerator DelayRequestFailedFade(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(failureSpawned);
    }

    private IEnumerator DelayRequestCompletedFade(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(completeSpawned);
    }

    private IEnumerator DelayForRandTime()
    {
        spawnCountFlag++;
        if (spawnCountFlag < 2)
        {
                randomDelay = UnityEngine.Random.Range(3f, 10f);
                yield return new WaitForSeconds(randomDelay);
                // to offset the failed request time to accomodate for the delay
                timer -= randomDelay;
                RaiseRequest();

        }
    }

    // Update is called once per frame
    void Update()
    {

        timer += Time.deltaTime;

            if (timer > 10 && timer < 11)
            {
                if (gameObject.tag == "NPC requester")
                {
                    StartCoroutine(DelayForRandTime());
                }
            }

            if (timer > 35 && timer < 36 && requestCompleted == false && requestRaised == true)
            {

                FailedRequest();
                timer = 0;
                spawnCountFlag = 0;
            }



    }

}
    
