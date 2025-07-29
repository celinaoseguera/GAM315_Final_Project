
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static GrowableSoil;


public class UIInventory : MonoBehaviour
{

    [SerializeField] TMP_Text AmountToChange;
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

        OnCropHarvestedText += ChangeTextIncrease;
        OnCropLostText += ChangeTextDecrease;
    }


    void HarvestListener(object sender, EventArgs e)
    {
        OnCropHarvestedText?.Invoke(this, new OnCropHarvestedTextArgs
        {
            text = AmountToChange
        });
    }

    void RequestListener(object sender, EventArgs e)
    {
        OnCropLostText?.Invoke(this, new OnCropLostTextArgs 
        {
            text = AmountToChange
        });
    }

    void ChangeTextIncrease(object sender, OnCropHarvestedTextArgs e)
    {
        currentAmount++;
        Debug.Log("text should have increased");
        e.text.text = currentAmount.ToString();
    }

    void ChangeTextDecrease(object sender, OnCropLostTextArgs e)
    {
        currentAmount--;
        Debug.Log("text should have decreased");
        e.text.text = currentAmount.ToString();
    }
}
