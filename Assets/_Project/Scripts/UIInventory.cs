
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static PlayerInventory;
using static GrowableSoil;
using static NPCFunctionality;
using static ShopFunctionality;


public class UIInventory : MonoBehaviour
{

    [SerializeField] TMP_Text WheatToChange;
    [SerializeField] TMP_Text MoneyToChange;
    [SerializeField] TMP_Text SeedToChange;
    [SerializeField] GameObject[] failureSprites;
    [SerializeField] NPCFunctionality[] npcFunctionalities;
    [SerializeField] GrowableSoil[] growableSoils;
    [SerializeField] ShopFunctionality shopKeeper;
    [SerializeField] PlayerInventory playerInventory;

    private SpriteRenderer spriteRenderer;
    private int failureCounter;



    void Start()
    {
        failureCounter = 0;


        foreach (NPCFunctionality script in npcFunctionalities)
        {
            script.OnFailure += ActivateFailureSprites;
            script.OnCropGiven += WheatChangeText;
            script.OnCropGiven += MoneyChangeText;
        }

        foreach (GrowableSoil script in growableSoils)
        {
            script.OnCropPlanted += SeedChangeText;
            script.OnCropHarvested += WheatChangeText;
        }

        shopKeeper.OnSeedsPurchased += MoneyChangeText;
        shopKeeper.OnSeedsPurchased += SeedChangeText;

    }



    void WheatChangeText(object sender, EventArgs e)
    {
        WheatToChange.text = playerInventory.wheatAmount.ToString(); ;
    }

    void MoneyChangeText(object sender, EventArgs e)
    {
        MoneyToChange.text = playerInventory.moneyAmount.ToString();
    }

    void SeedChangeText(object sender, EventArgs e)
    {
        SeedToChange.text = playerInventory.seedAmount.ToString();
    }


    void ActivateFailureSprites(object sender, EventArgs e)
    {
       
        failureSprites[failureCounter].GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        failureCounter++;
    }

}
