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

    // Update is called once per frame
    void Update()
    {
        // only call the water function attached to teh spacebar event IF the player is on top of the soil
        if (playerPresent == true)
        {
            inputPublisher.OnSpacePressed += WaterCrops;
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


    void WaterCrops(object sender, EventArgs e)
    {
        Debug.Log("plant watered");

    }

    //void HarvestCrops()

}
