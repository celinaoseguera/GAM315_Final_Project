using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static NPCFunctionality;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] NPCFunctionality[] npcRequesters;
    [SerializeField] GrowableSoil[] growableSoils;
    public int wheatAmount;
    public int moneyAmount;
    public int seedAmount;

    public event EventHandler OnCropReceived;
    public event EventHandler OnCropGiven;



    // Start is called before the first frame update
    void Start()
    {
        foreach (GrowableSoil script in growableSoils)
        {
            script.OnCropPlanted += subtractSeeds;
            script.OnCropHarvested += addCrops;
        }

        foreach (NPCFunctionality script in npcRequesters)
        {
            script.OnCropGiven += addMoney;
            script.OnCropGiven += subtractCrops;
            script.OnSeedsPurchased += addSeeds;
            script.OnSeedsPurchased += subtractMoney;
        }

        wheatAmount = 0;
        moneyAmount = 0;
        seedAmount = 6;

        
    }


    void RequestListener(object sender, EventArgs e)
    {
        OnCropGiven?.Invoke(this, EventArgs.Empty);
    }

   void addCrops(object sender, EventArgs e)
    {
        
        wheatAmount++;
    }

    void subtractCrops(object sender, EventArgs e)
    {
        wheatAmount--;
    }

    void addSeeds(object sender, OnSeedsPurchasedArgs e)
    {
        seedAmount = e.seedAmountPurchased;
    }

    void subtractSeeds(object sender, EventArgs e)
    {
        
        seedAmount--;
        Debug.Log(seedAmount + " after planting seed");

    }

    void addMoney (object sender, EventArgs e)
    {
        moneyAmount++;
    }

    void subtractMoney (object sender, OnSeedsPurchasedArgs e)
    {
        moneyAmount -= e.moneyToDeduct;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
