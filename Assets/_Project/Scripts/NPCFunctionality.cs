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
    private float timer;
    private bool requestRaised;
    private bool requestCompleted;
    private int failedNum;
    private Vector2 npcPos;
    private Vector2 npcPosOffsetY;
    private Vector2 npcPosOffsetXY;
    private const string REQUESTER_TAG = "NPC requester";

    public event EventHandler OnCropGiven;

    //NPC states
    [SerializeField] GameObject requestToSpawn;
    [SerializeField] GameObject failureToSpawn;
    [SerializeField] GameObject completeToSpawn;
    private GameObject requestSpawned;
    private GameObject failureSpawned;
    private GameObject completeSpawned;
    private int spawnCountFlag;
    private float randomDelay;

    public event EventHandler OnFailure;

    //NPC shopkeeper
    private const string SHOPKEEPER_TAG = "NPC shopkeeper";
    private int seedsAvailable;

    //NPC shopkeeper states
    [SerializeField] GameObject seedToSpawn;
    private GameObject seedSpawned;
    private bool seedsRaised;
    private bool seedsPurchaseComplete;
    private int seedToPlayerCount;
    
    public event EventHandler<OnSeedsPurchasedArgs> OnSeedsPurchased;

    public class OnSeedsPurchasedArgs : EventArgs
    {
        public int seedAmountPurchased;
        public int moneyToDeduct;
    }

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
        seedsRaised = false;
        seedsPurchaseComplete = false;
        seedToPlayerCount = 0;
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
            inputPublisher.OnEPressed += PurchaseSeeds;
        }

        return;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(PLAYER_TAG))
        {
            inputPublisher.OnEPressed -= GiveCrop;
            inputPublisher.OnEPressed -= PurchaseSeeds;
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
            OnCropGiven?.Invoke(this, EventArgs.Empty);
        }

    }

    private void PurchaseSeeds(object sender, EventArgs e)
    {
        if (seedsRaised == true && playerInventory.moneyAmount >= 1)
        {
            // if you have just enough for the seed batch, take all the seeds!
            if (seedsAvailable >= playerInventory.moneyAmount) 
            {
                seedToPlayerCount = seedsAvailable;
            }

            else
            {
                seedToPlayerCount = playerInventory.moneyAmount;
            }
            for (int i = 0; i <= seedToPlayerCount; i++)
            {
                OnSeedsPurchased?.Invoke(this, new OnSeedsPurchasedArgs
                {
                    seedAmountPurchased = seedToPlayerCount,
                    moneyToDeduct = playerInventory.moneyAmount
                });
            }
                // for loop to invoke OnMoneyGiuven and OnCropTaken (attached to addSeeds listener in PlayerInventory
                // and also in UInventory)
        }
    }

    void FailedRequest()

    {
        failedNum++;
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

    void RaiseSeeds()
    {
        seedSpawned = Instantiate(seedToSpawn, npcPosOffsetY, Quaternion.identity);
        seedsAvailable = UnityEngine.Random.Range(3, 12);
        Debug.Log(seedsAvailable + " amount of seeds availble");
        seedsPurchaseComplete = false;
        seedsRaised = true;
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

        if (gameObject.tag == "NPC requester")
        {


            if (timer > 10 && timer < 11)
            {
                if (gameObject.tag == "NPC requester")
                {
                    StartCoroutine(DelayForRandTime());
                }
            }

            if (timer > 20 && timer < 21 && requestCompleted == false && requestRaised == true)
            {

                FailedRequest();
                timer = 0;
                spawnCountFlag = 0;
            }

        }

        if (gameObject.tag == "NPC shopkeeper")
        {
            if (timer > 3 && timer < 4 && seedsPurchaseComplete == false && seedsRaised == false)
            {
                RaiseSeeds();

            }
        }


    }

}
    
