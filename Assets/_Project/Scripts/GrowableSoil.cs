using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static InputPublisher;
using static SpriteChanger;

public class GrowableSoil : MonoBehaviour
{
    // ground content
    [SerializeField] InputPublisher inputPublisher;
    private SpriteChanger scriptToAccess;
    [SerializeField] GameObject cropToAccessVars;
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



    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        plotPos = this.transform.position;
        //gets the crop's SpriteChanger script and its vars
        scriptToAccess = cropToAccessVars.GetComponent<SpriteChanger>();
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
        if (scriptToAccess.cropPlanted == false) {
            OnCropPlanted?.Invoke(this, new OnCropPlantedArgs
        {
            soilPlotPos = plotPos
        });

    }
        // will show the seed sprite at location of trhe soil plot
    }

    void WaterCrops(object sender, EventArgs e)
    {
        if (scriptToAccess.cropWatered == false)
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
        if (scriptToAccess.cropReadyForHarvest == true && scriptToAccess.cropPlanted == true)
        {
            OnCropHarvested?.Invoke(this, EventArgs.Empty);
        }

    }

    void Update()
    {
        if (scriptToAccess.cropWatered == false && scriptToAccess.cropReadyForHarvest == true)
        { 
        spriteRenderer.color = new Color(0f, 0.5019608f, 0.5019608f, 0f);
        }

        }
    

    

}
