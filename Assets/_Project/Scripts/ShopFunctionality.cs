using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopFunctionality : MonoBehaviour
{

    //NPC shopkeeper
    private int seedsAvailable;

    //NPC shopkeeper states
    [SerializeField] InputPublisher inputPublisher;
    [SerializeField] GameObject seedToSpawn;
    private GameObject seedSpawned;
    private bool seedsRaised;
    private bool seedsPurchaseComplete;
    private int spawnCountFlag;
    private float timer;
    private Vector2 npcPos;
    private Vector2 npcPosOffsetY;
    private Vector2 npcPosOffsetXY;

    public event EventHandler<OnSeedsPurchasedArgs> OnSeedsPurchased;

    public class OnSeedsPurchasedArgs : EventArgs
    {
        public int seedAmountPurchased;
        public int moneyToDeduct;
    }

    //player inventory
    [SerializeField] PlayerInventory playerInventory;
    private const string PLAYER_TAG = "Player";

    void Start()
    {
        npcPos = this.transform.position;
        npcPosOffsetXY = new Vector2(npcPos.x + .4f, npcPos.y + 1.9f);
        seedsRaised = false;
        seedsPurchaseComplete = false;
        timer = 0;
        spawnCountFlag  = 0;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(PLAYER_TAG))
        {
            inputPublisher.OnEPressed += PurchaseSeeds;
        }

        return;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(PLAYER_TAG))
        {
            inputPublisher.OnEPressed -= PurchaseSeeds;
        }

        return;
    }

    private void PurchaseSeeds(object sender, EventArgs e)
    {
        if (seedsRaised == true && playerInventory.moneyAmount >= 1)
        {
            OnSeedsPurchased?.Invoke(this, new OnSeedsPurchasedArgs
            {
                seedAmountPurchased = seedsAvailable,
                moneyToDeduct = seedsAvailable
            });

            timer = 0;
            spawnCountFlag = 0;
            Destroy(seedSpawned);
            seedsRaised = false;
            // for loop to invoke OnMoneyGiuven and OnCropTaken (attached to addSeeds listener in PlayerInventory
            // and also in UInventory)
        }
    }

    void RaiseSeeds()
    {
        seedSpawned = Instantiate(seedToSpawn, npcPosOffsetXY, Quaternion.identity);
        seedsAvailable = playerInventory.moneyAmount;
        seedsPurchaseComplete = false;
        seedsRaised = true;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 30 && seedsPurchaseComplete == false && seedsRaised == false && playerInventory.moneyAmount >= 1)
        {
            spawnCountFlag++;

            if (spawnCountFlag < 2)
            {
                RaiseSeeds();
            }
        }
    }
}
    
