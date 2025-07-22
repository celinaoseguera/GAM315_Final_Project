using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GrowableSoil;

public class CropGrowth : MonoBehaviour
{
    [SerializeField] GrowableSoil cropPublisher;
    [SerializeField] Sprite[] cropSprites;
    [SerializeField] Sprite newSprite;

    // Start is called before the first frame update
    void Start()
    {
        cropPublisher.OnCropPlanted += cropToSeed;
       

    }

    private void cropToSeed(object sender, EventArgs e)
    {
        newSprite = cropSprites[0];

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
