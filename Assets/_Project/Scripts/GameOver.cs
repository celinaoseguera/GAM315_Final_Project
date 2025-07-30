using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static NPCFunctionality;

public class GameOver : MonoBehaviour
{

    [SerializeField] NPCFunctionality[] npcFunctionalities;
    private int failureCount;

    void Start()
    {
        foreach (NPCFunctionality script in npcFunctionalities)
        {
            script.OnFailure += addToFailureCount;
        }
        
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
            Debug.Log("GAME OVER");
            Time.timeScale = 0f;
        }
        
    }
}
