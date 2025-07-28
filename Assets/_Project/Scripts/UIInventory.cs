
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static GrowableSoil;


public class UIInventory : MonoBehaviour
{

    [SerializeField] TMP_Text AmountToChange;
    [SerializeField] GrowableSoil[] growableSoils;
    private int currentAmount;

    public event EventHandler<OnCropHarvestedTextArgs> OnCropHarvestedText;
    public class OnCropHarvestedTextArgs : EventArgs
    {
        public TMP_Text text;
    }

    void Start()
    {
        currentAmount += 0;
        foreach (GrowableSoil script in growableSoils)
        {
            script.OnCropHarvested += HarvestListener;
        }
        OnCropHarvestedText += ChangeText;
    }


    void HarvestListener(object sender, EventArgs e)
    {
        OnCropHarvestedText?.Invoke(this, new OnCropHarvestedTextArgs
        {
            text = AmountToChange
        });


    }

    void ChangeText(object sender, OnCropHarvestedTextArgs e)
    {
        currentAmount++;
        Debug.Log("text should have changed");
        e.text.text = currentAmount.ToString();
    }
}
