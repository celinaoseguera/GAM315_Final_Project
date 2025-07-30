
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static GrowableSoil;


public class UIInventory : MonoBehaviour
{

    [SerializeField] TMP_Text WheatToChange;
    [SerializeField] TMP_Text MoneyToChange;
    [SerializeField] NPCFunctionality[] npcFunctionalities;
    [SerializeField] GrowableSoil[] growableSoils;
    private int currentAmount;

    public event EventHandler<OnCropHarvestedTextArgs> OnCropHarvestedText;
    public class OnCropHarvestedTextArgs : EventArgs
    {
        public TMP_Text text;
    }

    public event EventHandler<OnCropLostTextArgs> OnCropLostText;
    public class OnCropLostTextArgs : EventArgs
    {
        public TMP_Text text;
    }

    void Start()
    {
        currentAmount = 0;
        foreach (GrowableSoil script in growableSoils)
        {
            script.OnCropHarvested += HarvestListener;
        }

        foreach (NPCFunctionality script in npcFunctionalities)
        {
            script.OnCropGiven += RequestListener;
        }

        OnCropHarvestedText += WheatChangeTextIncrease;
        OnCropLostText += WheatChangeTextDecrease;
        OnCropLostText += MoneyChangeTextIncrease;
    }


    void HarvestListener(object sender, EventArgs e)
    {
        OnCropHarvestedText?.Invoke(this, new OnCropHarvestedTextArgs
        {
            text = WheatToChange
        });
    }

    void RequestListener(object sender, EventArgs e)
    {
        OnCropLostText?.Invoke(this, new OnCropLostTextArgs 
        {
            text = WheatToChange
        });

        OnCropLostText?.Invoke(this, new OnCropLostTextArgs
        {
            text = MoneyToChange
        });

    }

    void WheatChangeTextIncrease(object sender, OnCropHarvestedTextArgs e)
    {
        currentAmount++;
        e.text.text = currentAmount.ToString();
    }

    void WheatChangeTextDecrease(object sender, OnCropLostTextArgs e)
    {
        currentAmount--;
        e.text.text = currentAmount.ToString();
    }

    void MoneyChangeTextIncrease(object sender, OnCropLostTextArgs e)
    {
        currentAmount++;
        e.text.text = currentAmount.ToString();
    }
}
