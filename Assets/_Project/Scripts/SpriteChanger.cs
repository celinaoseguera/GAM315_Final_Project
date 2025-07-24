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
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = null;
        growableSoil.OnCropPlanted += cropToSeed;
        growableSoil.OnCropWatered += cropToGrow;
    }

    // Update is called once per frame
    void Update()
    {
        if (cropWatered == true && cropPlanted == true)
        {
            timer += Time.deltaTime;

            if (timer > 5 && timer < 9 )
            {
                Debug.Log("should be plant now");
                newSprite = cropSprites[1];
                gameObject.GetComponent<SpriteRenderer>().sprite = newSprite;
            }

            if (timer > 10 )
            {
                newSprite = cropSprites[2];
                gameObject.GetComponent<SpriteRenderer>().sprite = newSprite;
                cropWatered = false;
                timer = 0;
            }
        }
        
    }

    private void cropToSeed(object sender, GrowableSoil.OnCropPlantedArgs e)
    {
        cropPlanted = true;
        newSprite = cropSprites[0];
        gameObject.GetComponent<SpriteRenderer>().sprite = newSprite;
        Instantiate(this, e.soilPlotPos, Quaternion.identity);

    }

    private void cropToGrow(object sender, GrowableSoil.OnCropWateredArgs e)
    {
        if (cropWatered == false)
        {

            Debug.Log("crop watered in soiulchanger");
            cropWatered = true;
        }
        

    }
}
