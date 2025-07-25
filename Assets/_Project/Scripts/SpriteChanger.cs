using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GrowableSoil;

public class SpriteChanger : MonoBehaviour
{
    [SerializeField] Sprite[] cropSprites;
    [SerializeField] GrowableSoil growableSoil;
    public Sprite newSprite;
    private float timer;
    private bool cropWatered = false;
    private bool cropPlanted = false;
    public bool cropReadyForHarvest = false;
    // Start is called before the first frame update
    void Awake()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = null;
        growableSoil.OnCropPlanted += CropToSeed;
        growableSoil.OnCropWatered += CropToGrow;
        growableSoil.OnCropHarvested += CropToBeHarvested;
    }

    // Update is called once per frame
    void Update()
    {
        if (cropWatered == true && cropPlanted == true)
        {
            timer += Time.deltaTime;

            if (timer > 5 && timer < 9 )
            {
                newSprite = cropSprites[1];
                gameObject.GetComponent<SpriteRenderer>().sprite = newSprite;
            }

            if (timer > 10 )
            {
                newSprite = cropSprites[2];
                gameObject.GetComponent<SpriteRenderer>().sprite = newSprite;
                cropWatered = false;
                cropReadyForHarvest = true;
                timer = 0;
            }
        }
        
    }

    private void CropToSeed(object sender, GrowableSoil.OnCropPlantedArgs e)
    {
        cropPlanted = true;
        Debug.Log("crop planted");
        newSprite = cropSprites[0];
        gameObject.GetComponent<SpriteRenderer>().sprite = newSprite;
        Instantiate(this, e.soilPlotPos, Quaternion.identity);

    }

    private void CropToGrow(object sender, GrowableSoil.OnCropWateredArgs e)
    {
            cropWatered = true;
            // being called twice for some reason
            Debug.Log("crop watered");
    }

    void CropToBeHarvested(object sender, EventArgs e)
    {
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = null;
            // being called twice for some reason
            Debug.Log("crop harvested!");
        }

    }
}
