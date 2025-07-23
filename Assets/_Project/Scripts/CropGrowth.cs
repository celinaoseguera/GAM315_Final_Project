using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GrowableSoil;

public class CropGrowth : MonoBehaviour
{
    public GameObject cropToSpawn;
    [SerializeField] GrowableSoil cropPublisher;
    //[SerializeField] Sprite[] cropSprites;
    //[SerializeField] Sprite newSprite;

    // Start is called before the first frame update
    void Start()
    {
        cropPublisher.OnCropPlanted += cropToSeed;
       

    }

    private void cropToSeed(object sender, GrowableSoil.OnCropPlantedArgs e)
    {
        Instantiate(cropToSpawn,e.soilPlotPos, Quaternion.identity);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
