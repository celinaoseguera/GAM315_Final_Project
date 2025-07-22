using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static InputPublisher;

public class GrowableSoil : MonoBehaviour
{
    [SerializeField] InputPublisher inputPublisher;
    private bool playerPresent = false;
    private const string PLAYER_TAG = "Player";
    private SpriteRenderer spriteRenderer;
    private bool cropWatered = false;

    public event EventHandler OnCropPlanted;
    public event EventHandler OnCropWatered;
    //public event EventHandler OnCropHarvested;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // only call the water function attached to teh spacebar event IF the player is on top of the soil
        // CELINA, do a check for if the plant has already been watered otherwise will put too many calls into callback
        if (playerPresent == true && cropWatered == false)
        {
            inputPublisher.OnEPressed += PlantCrops;
            inputPublisher.OnSpacePressed += WaterCrops;
            cropWatered = true;

            // inputPublisher.OnSpacePressed += HarvestCrops;
        }

    }

    // when player touches the soil trigger

    private void OnTriggerEnter2D (Collider2D other)
    {
        if (other.gameObject.CompareTag(PLAYER_TAG))
        {
            playerPresent = true;
            Debug.Log("in the plot!");
        }

        return;
    }

    private void OnTriggerExit2D (Collider2D other)
    {
        if (other.gameObject.CompareTag(PLAYER_TAG))
        {
            playerPresent = false;
            Debug.Log("left the plot!");
        }

        return;
    }

    void PlantCrops(object sender, EventArgs e)
    {
        Debug.Log("planted!");
        OnCropPlanted?.Invoke(this, EventArgs.Empty);
        // will show the seed sprite
    }

    void WaterCrops(object sender, EventArgs e)
    {
        Debug.Log("plant watered");
        OnCropWatered?.Invoke(this, EventArgs.Empty);
        // will set off time delta time clock to change sprite from seed to plant and then to mature
        spriteRenderer.color = new Color (0f, 0.5019608f, 0.5019608f, .3f);

    }

    //void HarvestCrops()

}
