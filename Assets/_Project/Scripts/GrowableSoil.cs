using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static InputPublisher;

public class GrowableSoil : MonoBehaviour
{
    [SerializeField] InputPublisher inputPublisher;
    private const string PLAYER_TAG = "Player";
    private SpriteRenderer spriteRenderer;

    public event EventHandler OnCropPlanted;
    public event EventHandler OnCropWatered;
    //public event EventHandler OnCropHarvested;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }



    private void OnTriggerEnter2D (Collider2D other)
    {
        if (other.gameObject.CompareTag(PLAYER_TAG))
        {
            inputPublisher.OnEPressed += PlantCrops;
            inputPublisher.OnSpacePressed += WaterCrops;
            Debug.Log("in the plot!");
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
        OnCropPlanted?.Invoke(this, EventArgs.Empty);
        // will show the seed sprite
    }

    void WaterCrops(object sender, EventArgs e)
    {
        OnCropWatered?.Invoke(this, EventArgs.Empty);
        // will set off time delta time clock to change sprite from seed to plant and then to mature
        spriteRenderer.color = new Color (0f, 0.5019608f, 0.5019608f, .3f);

    }

    //void HarvestCrops()

}
