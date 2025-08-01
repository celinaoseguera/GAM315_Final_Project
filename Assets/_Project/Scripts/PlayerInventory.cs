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
            script.OnCropHarvested += HarvestListener;
        }

        foreach (NPCFunctionality script in npcRequesters)
        {
            script.OnCropGiven += RequestListener;
            script.OnSeedsPurchased += addSeeds;
            script.OnSeedsPurchased += subtractMoney;
        }

        OnCropGiven += subtractCrops;
        OnCropReceived += addCrops;
        OnCropReceived += addMoney;
        wheatAmount = 0;
        moneyAmount = 10;
        seedAmount = 0;

        
    }

    void HarvestListener(object sender, EventArgs e)
    {
       OnCropReceived?.Invoke(this, EventArgs.Empty);
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
        Debug.Log(seedAmount + " after purchasing seeds");
    }

    void subtractSeeds(object sender, EventArgs e)
    {
        seedAmount--;
        Debug.Log(seedAmount + " after planting seeds");

    }

    void addMoney (object sender, EventArgs e)
    {
        moneyAmount++;
    }

    void subtractMoney (object sender, OnSeedsPurchasedArgs e)
    {
        moneyAmount -= e.moneyToDeduct;
        Debug.Log(moneyAmount + " after purchasing seeds");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
