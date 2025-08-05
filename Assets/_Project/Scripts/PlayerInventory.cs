using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static NPCFunctionality;
using static ShopFunctionality;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] NPCFunctionality[] npcRequesters;
    [SerializeField] GrowableSoil[] growableSoils;
    [SerializeField] ShopFunctionality shopKeeper;
    public int wheatAmount;
    public int moneyAmount;
    public int seedAmount;
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
        }

        shopKeeper.OnSeedsPurchased += addSeeds;
        shopKeeper.OnSeedsPurchased += subtractMoney;
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

    }

    void addMoney (object sender, EventArgs e)
    {
        moneyAmount++;
    }

    void subtractMoney (object sender, OnSeedsPurchasedArgs e)
    {
        moneyAmount -= e.moneyToDeduct;
    }

}
