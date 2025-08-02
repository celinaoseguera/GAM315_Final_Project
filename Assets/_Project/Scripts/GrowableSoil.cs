using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static InputPublisher;
using static SpriteChanger;
using TMPro;

public class GrowableSoil : MonoBehaviour
{
    // ground content
    [SerializeField] InputPublisher inputPublisher;
    private const string PLAYER_TAG = "Player";
    private Vector2 plotPos;
    private SpriteRenderer spriteRenderer;
    private float timer;

    public event EventHandler<OnCropPlantedArgs> OnCropPlanted;
    public class OnCropPlantedArgs : EventArgs
    {
        public Vector2 soilPlotPos;
    }

    public event EventHandler<OnCropWateredArgs> OnCropWatered;
    public class OnCropWateredArgs: EventArgs
    {
        public Vector2 soilPlotPos;
    }
    
    public event EventHandler OnCropHarvested;


    // crop content
    [SerializeField] GameObject cropToSpawn;
    [SerializeField] SpriteChanger spriteChanger;
    private GameObject spawnedCrop;
    private bool cropPlanted;
    private bool cropWatered;
    private bool cropReadyForHarvest;
    public bool cropHarvested;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        plotPos = this.transform.position;
        //gets the crop's SpriteChanger script and its vars
        //scriptToAccess = cropToSpawn.GetComponent<SpriteChanger>();
        OnCropPlanted += CropToSeed;
        OnCropWatered += CropToGrow;
        OnCropHarvested += CropToBeHarvested;
        cropPlanted = false;
        cropReadyForHarvest = false;
        cropHarvested = false;
    }



    private void OnTriggerEnter2D (Collider2D other)
    {
        if (other.gameObject.CompareTag(PLAYER_TAG))
        {
            inputPublisher.OnEPressed += PlantCrops;
            inputPublisher.OnSpacePressed += WaterCrops;
            inputPublisher.OnEPressed += HarvestCrops;
        }

        return;
    }

    private void OnTriggerExit2D (Collider2D other)
    {
        if (other.gameObject.CompareTag(PLAYER_TAG))
        {
            inputPublisher.OnEPressed -= PlantCrops;
            inputPublisher.OnSpacePressed -= WaterCrops;
            inputPublisher.OnEPressed -= HarvestCrops;
        }

        return;
    }

    void PlantCrops(object sender, EventArgs e)
    {
        if (cropPlanted == false) {
            OnCropPlanted?.Invoke(this, new OnCropPlantedArgs
        {
            soilPlotPos = plotPos
        });

    }
        // will show the seed sprite at location of trhe soil plot
    }

    void WaterCrops(object sender, EventArgs e)
    {
        if (cropWatered == false)
        {
            OnCropWatered?.Invoke(this, new OnCropWateredArgs
            {
                soilPlotPos = plotPos
            });
            // will set off time delta time clock to change sprite from seed to plant and then to mature
            spriteRenderer.color = new Color(0f, 0.5019608f, 0.5019608f, .3f);
        }

    }

    void HarvestCrops(object sender, EventArgs e)
    {
        if (cropReadyForHarvest == true && cropPlanted == true)
        {
            OnCropHarvested?.Invoke(this, EventArgs.Empty);
        }

    }

    private void CropToSeed(object sender, GrowableSoil.OnCropPlantedArgs e)
    {
        cropPlanted = true;
        cropToSpawn.GetComponent<SpriteRenderer>().sprite = spriteChanger.cropSprites[0];
        spawnedCrop = Instantiate(cropToSpawn, e.soilPlotPos, Quaternion.identity);

    }

    private void CropToGrow(object sender, GrowableSoil.OnCropWateredArgs e)
    {
        cropWatered = true;
    }

    public void CropToBeHarvested(object sender, EventArgs e)
    {
        {
            Destroy(spawnedCrop);
            cropHarvested = true;
            cropPlanted = false;
            cropReadyForHarvest = false;


            
        }

    }

    void Update()
    {

        if (cropWatered == true && cropPlanted == true)
        {
            timer += Time.deltaTime;

            if (timer > 5 && timer < 9)
            {
                spawnedCrop.GetComponent<SpriteRenderer>().sprite = spriteChanger.cropSprites[1];
            }

            if (timer > 10)
            {
                spawnedCrop.GetComponent<SpriteRenderer>().sprite = spriteChanger.cropSprites[2];
                cropWatered = false;
                cropReadyForHarvest = true;
                spriteRenderer.color = new Color(0f, 0.5019608f, 0.5019608f, 0f);
                timer = 0;
            }
        }

    }
    

    

}
