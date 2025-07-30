
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
    [SerializeField] GameObject[] failureSprites;
    [SerializeField] NPCFunctionality[] npcFunctionalities;
    [SerializeField] GrowableSoil[] growableSoils;

    private int currentAmount;
    private SpriteRenderer spriteRenderer;
    private int failureCounter;

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
        failureCounter = 0;
        currentAmount = 0;
        foreach (GrowableSoil script in growableSoils)
        {
            script.OnCropHarvested += HarvestListener;
        }

        foreach (NPCFunctionality script in npcFunctionalities)
        {
            script.OnCropGiven += RequestListener;
        }

        foreach (NPCFunctionality script in npcFunctionalities)
        {
            script.OnFailure += activateFailureSprites;
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

    void activateFailureSprites(object sender, EventArgs e)
    {
       
        failureSprites[failureCounter].GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        failureCounter++;
    }
}
