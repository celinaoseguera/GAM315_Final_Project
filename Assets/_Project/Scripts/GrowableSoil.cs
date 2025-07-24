using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static InputPublisher;

public class GrowableSoil : MonoBehaviour
{
    // ground content
    [SerializeField] InputPublisher inputPublisher;
    private const string PLAYER_TAG = "Player";
    private Vector2 plotPos;
    private SpriteRenderer spriteRenderer;
    private bool plotOcupado = false;
    private bool plotHumedo = false;
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
    //public event EventHandler OnCropHarvested;



    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        plotPos = this.transform.position;
    }



    private void OnTriggerEnter2D (Collider2D other)
    {
        if (other.gameObject.CompareTag(PLAYER_TAG))
        {
            inputPublisher.OnEPressed += PlantCrops;
            inputPublisher.OnSpacePressed += WaterCrops;
        }

        return;
    }

    private void OnTriggerExit2D (Collider2D other)
    {
        if (other.gameObject.CompareTag(PLAYER_TAG))
        {
            inputPublisher.OnEPressed -= PlantCrops;
            inputPublisher.OnSpacePressed -= WaterCrops;;
        }

        return;
    }

    void PlantCrops(object sender, EventArgs e)
    {
        if (plotOcupado == false) {
        OnCropPlanted?.Invoke(this, new OnCropPlantedArgs
        {
            soilPlotPos = plotPos
        });

        plotOcupado = true;
    }
        // will show the seed sprite at location of trhe soil plot
    }

    void WaterCrops(object sender, EventArgs e)
    {
        if (plotHumedo == false) {
            OnCropWatered?.Invoke(this, new OnCropWateredArgs
            {
                soilPlotPos = plotPos
            });
            // will set off time delta time clock to change sprite from seed to plant and then to mature
            spriteRenderer.color = new Color(0f, 0.5019608f, 0.5019608f, .3f);
            plotHumedo = true;
        }

    }

    void Update()
    {
        if (plotHumedo == true)
        {

            timer += Time.deltaTime;

            if (timer > 10)
            {
                spriteRenderer.color = new Color(0f, 0.5019608f, 0.5019608f, 0f);
                timer = 0;
                plotHumedo = false;
            }

        }
    }

    //void HarvestCrops()

}
