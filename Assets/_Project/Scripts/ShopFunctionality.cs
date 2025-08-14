using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopFunctionality : MonoBehaviour
{

    //NPC shopkeeper
    private int seedsAvailable;

    //NPC shopkeeper states
    [SerializeField] InputPublisher inputPublisher;
    [SerializeField] GameObject seedToSpawn;
    [SerializeField] TMP_Text availableSeedsNum;
    [SerializeField] TMP_Text availableSeedsTxt;
    [SerializeField] GameObject availableBox;
    private GameObject seedSpawned;
    private bool seedsRaised;
    private bool seedsPurchaseComplete;
    private int spawnCountFlag = 0;
    private float timer = 0;
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

    // audio
    [SerializeField] private AudioClip moneySoundClip;
    [SerializeField] private AudioClip raiseSeedsSoundClip;

    void Start()
    {
        npcPos = this.transform.position;
        npcPosOffsetXY = new Vector2(npcPos.x + .4f, npcPos.y + 1.9f);
        seedsRaised = false;
        seedsPurchaseComplete = false;
        
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
            SoundFXManager.instance.PlaySoundFXClip(moneySoundClip, transform, 1f);
            availableBox.SetActive(false);
            seedsRaised = false;
        }
    }

    void RaiseSeeds()
    {
        seedSpawned = Instantiate(seedToSpawn, npcPosOffsetXY, Quaternion.identity);
        SoundFXManager.instance.PlaySoundFXClip(raiseSeedsSoundClip, transform, 1f);
        seedsAvailable = playerInventory.moneyAmount;
        availableBox.SetActive(true);
        availableSeedsNum.text = playerInventory.moneyAmount.ToString();
        availableSeedsTxt.text = "available";
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
    
