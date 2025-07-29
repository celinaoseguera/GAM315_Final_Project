using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static InputPublisher;

public class PlayerController : MonoBehaviour
{

    [SerializeField] InputPublisher inputPublisher;
    float movementSpeed = 30.0f;
    // assigning keyPressed arg to an var
    InputPublisher.OnWASDPressedEventArgs e;
    private Rigidbody2D rigidBody;
    private Vector2 targetPos;
 


    // Start is called before the first frame update
    void Start()
    {
        // subscribe eventHanlder to event at start
        inputPublisher.OnWASDPressed += Direction;
        rigidBody = this.GetComponent<Rigidbody2D>();
        

    }



    // Eventhandler function

    void Direction(object sender, InputPublisher.OnWASDPressedEventArgs e)
    {


        switch (e.keyPressed)
        {
            case "A":
                rigidBody.MovePosition(rigidBody.position + (Vector2.left * movementSpeed * Time.deltaTime));
                break;
            case "D":
                rigidBody.MovePosition(rigidBody.position + (Vector2.right * movementSpeed * Time.deltaTime));
                break;
            case "W":
                rigidBody.MovePosition(rigidBody.position + (Vector2.up * movementSpeed * Time.deltaTime));
                break;
            case "S":
                rigidBody.MovePosition(rigidBody.position + (Vector2.down * movementSpeed * Time.deltaTime));
                break;
        }

    }

}
