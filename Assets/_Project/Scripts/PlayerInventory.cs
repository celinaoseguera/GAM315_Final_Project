using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static NPCFunctionality;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] NPCFunctionality[] npcFunctionalities;
    [SerializeField] GrowableSoil[] growableSoils;
    public int wheatAmount;
    public int moneyAmount;

    public event EventHandler OnCropReceived;
    public event EventHandler OnCropLost;


    // Start is called before the first frame update
    void Start()
    {
        foreach (GrowableSoil script in growableSoils)
        {
            script.OnCropHarvested += HarvestListener;
        }

        foreach (NPCFunctionality script in npcFunctionalities)
        {
            script.OnCropGiven += RequestListener;
        }

        OnCropLost += subtractCrops;
        OnCropReceived += addCrops;
        wheatAmount = 0;
        moneyAmount = 0;
        
    }

    void HarvestListener(object sender, EventArgs e)
    {
       OnCropReceived?.Invoke(this, EventArgs.Empty);
    }

    void RequestListener(object sender, EventArgs e)
    {
        OnCropLost?.Invoke(this, EventArgs.Empty);
    }

   void addCrops(object sender, EventArgs e)
    {
        wheatAmount++;
        Debug.Log(wheatAmount);
    }

    void subtractCrops(object sender, EventArgs e)
    {
        wheatAmount--;
        Debug.Log(wheatAmount);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
