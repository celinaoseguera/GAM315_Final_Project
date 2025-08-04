using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static NPCFunctionality;

public class GameOver : MonoBehaviour
{

    [SerializeField] NPCFunctionality[] npcFunctionalities;
    [SerializeField] TMP_Text gameOverTitle;
    [SerializeField] GameObject gameOverScreen;
    private int failureCount;



    void Start()
    {
        foreach (NPCFunctionality script in npcFunctionalities)
        {
            script.OnFailure += addToFailureCount;
        }

        gameOverTitle.text = "";
        
    }

    public void addToFailureCount(object sender, EventArgs e)
    {
        failureCount++;
    }

    // Update is called once per frame
    void Update()
    {
        if (failureCount == 5)
        {
            gameOverTitle.text = "GAME OVER";
            gameOverScreen.SetActive(true);
            Time.timeScale = 0f;
        }
        
    }
}
