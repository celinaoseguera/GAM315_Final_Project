using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static NPCFunctionality;

public class GameOver : MonoBehaviour
{

    [SerializeField] NPCFunctionality[] npcFunctionalities;
    [SerializeField] TMP_Text gameOverTitle;
    [SerializeField] TMP_Text gameOverCompleted;
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] Button restartBtn;
    private int failureCount = 0;
    private int completedCount = 0;



    void Start()
    {
        foreach (NPCFunctionality script in npcFunctionalities)
        {
            script.OnFailure += addToFailureCount;
            script.OnCompleted += addToCompletedCount;
        }

        gameOverTitle.text = "";
        gameOverCompleted.text = "";

    }

    public void addToFailureCount(object sender, EventArgs e)
    {
        failureCount++;
    }

    public void addToCompletedCount(object sender, EventArgs e)
    {
        completedCount++;
    }

    void Update()
    {
        if (failureCount == 5)
        {
            gameOverScreen.SetActive(true);
            restartBtn.gameObject.SetActive(true);
            gameOverTitle.text = "GAME OVER";
            gameOverCompleted.text = "Requests completed: " + completedCount.ToString();
            Time.timeScale = 0f;
        }
        

    }
}
