using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static InputPublisher;
using static PlayerInventory;


public class NPCFunctionality : MonoBehaviour
{
    //NPC
    [SerializeField] InputPublisher inputPublisher;
    private float timer;
    private bool requestRaised;
    private bool requestCompleted;
    private int failedNum;
    private Vector2 npcPos;
    private Vector2 npcPosOffsetY;
    private Vector2 npcPosOffsetXY;

    public event EventHandler OnCropGiven;

    //State
    [SerializeField] GameObject requestToSpawn;
    [SerializeField] GameObject failureToSpawn;
    [SerializeField] GameObject completeToSpawn;
    private GameObject requestSpawned;
    private GameObject failureSpawned;
    private GameObject completeSpawned;
    private int spawnCountFlag;
    private float randomDelay;

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
        randomDelay = 0;

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
            Debug.Log("Completed");
            requestRaised = false;
            timer = 0;
            spawnCountFlag = 0;
            OnCropGiven?.Invoke(this, EventArgs.Empty);
        }

    }

    void FailedRequest()

    {
        Debug.Log("Failed");
        failedNum++;
        failureSpawned = Instantiate(failureToSpawn, npcPosOffsetXY, Quaternion.identity);
        StartCoroutine(DelayRequestFailedFade(2f));
        StartCoroutine(DelayRequestFade(2f));
        requestRaised = false;
    }

    void RaiseRequest()
    {
        Debug.Log("Raised");
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

    private IEnumerator DelayForRandTime(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        spawnCountFlag++;
        if (spawnCountFlag < 2)
        {
            Debug.Log(randomDelay);
            RaiseRequest();

        }
    }

    // Update is called once per frame
    void Update()
    {

        timer += Time.deltaTime;

        if (timer > 10 && timer < 11)
        {
            randomDelay = UnityEngine.Random.Range(3, 10);
            StartCoroutine(DelayForRandTime(randomDelay));

        }

        if (timer > 20 && timer < 21 && requestCompleted == false && requestRaised == true)
        {
    
            FailedRequest();
            timer = 0;
            spawnCountFlag = 0;
        }

        if (failedNum == 3)

        {
            Debug.Log("GAME OVER");
            Time.timeScale = 0f;
        }
            
        }
    
}
    
