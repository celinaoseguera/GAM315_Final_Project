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
    public event EventHandler OnCropPlanted;
    public event EventHandler OnCropWatered;
    public event EventHandler OnCropHarvested;

    // ground content
    [SerializeField] InputPublisher inputPublisher;
    private const string PLAYER_TAG = "Player";
    private Vector2 plotPos;
    private Vector2 waterPos;
    private Vector2 harvestPos;
    private SpriteRenderer spriteRenderer;
    private float timer;


    // crop content
    [SerializeField] GameObject cropToSpawn;
    [SerializeField] GameObject waterToSpawn;
    [SerializeField] GameObject harvestToSpawn;
    [SerializeField] SpriteChanger spriteChanger;
    [SerializeField] PlayerInventory playerInventory;
    private GameObject spawnedCrop;
    private GameObject spawnedWater;
    private GameObject spawnedHarvest;
    private bool cropPlanted = false;
    private bool cropWatered;
    private bool cropReadyForHarvest = false;
    public bool cropHarvested = false;

    // audio
    [SerializeField] private AudioClip waterSoundClip;
    [SerializeField] private AudioClip harvestSoundClip;
    [SerializeField] private AudioClip cropReadySoundClip;


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        plotPos = this.transform.position;
        waterPos = new Vector2 (plotPos.x, plotPos.y +.01f);
        harvestPos = new Vector2(plotPos.x, plotPos.y + 1f);
    }



    private void OnTriggerEnter2D (Collider2D other)
    {
        if (other.gameObject.CompareTag(PLAYER_TAG))
        {
            inputPublisher.OnEPressed += PlantCrop;
            inputPublisher.OnSpacePressed += WaterCrop;
            inputPublisher.OnEPressed += HarvestCrop;
        }

        return;
    }

    private void OnTriggerExit2D (Collider2D other)
    {
        if (other.gameObject.CompareTag(PLAYER_TAG))
        {
            inputPublisher.OnEPressed -= PlantCrop;
            inputPublisher.OnSpacePressed -= WaterCrop;
            inputPublisher.OnEPressed -= HarvestCrop;
        }

        return;
    }

    private void PlantCrop(object sender, EventArgs e)
    {
        if (cropPlanted == false && playerInventory.seedAmount >= 1)
        {
            OnCropPlanted?.Invoke(this, EventArgs.Empty);
            cropPlanted = true;
            cropToSpawn.GetComponent<SpriteRenderer>().sprite = spriteChanger.cropSprites[0];
            spawnedCrop = Instantiate(cropToSpawn, plotPos, Quaternion.identity);

            if (cropWatered == false)
            {
                spawnedWater = Instantiate(waterToSpawn, waterPos, Quaternion.identity);
            }
        }

    }

    private void WaterCrop(object sender, EventArgs e)
    {
        if (cropWatered == false)
        {
            OnCropWatered?.Invoke(this, EventArgs.Empty);
            spriteRenderer.color = new Color(0f, 0.5019608f, 0.5019608f, .6f);
            SoundFXManager.instance.PlaySoundFXClip(waterSoundClip, transform, 1f);
            Destroy(spawnedWater);
        }
        cropWatered = true;
    }

    public void HarvestCrop(object sender, EventArgs e)
    { 
        if (cropReadyForHarvest == true && cropPlanted == true)
        {
            OnCropHarvested?.Invoke(this, EventArgs.Empty);
            SoundFXManager.instance.PlaySoundFXClip(harvestSoundClip, transform, 1f);
            Destroy(spawnedCrop);
            Destroy(spawnedHarvest);
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
                spawnedHarvest = Instantiate(harvestToSpawn, harvestPos, Quaternion.identity);
                SoundFXManager.instance.PlaySoundFXClip(cropReadySoundClip, transform, 1f);
                timer = 0;
            }
        }

    }
    

    

}
