using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InputPublisher : MonoBehaviour
{
    // create events
        // create class for args if applicable
    // raise events (in update)
    // handle events over in whoever is subscribing to the event being raised (separate files)

    public event EventHandler <OnWASDPressedEventArgs> OnWASDPressed; 

    public class OnWASDPressedEventArgs: EventArgs
    {
        public string keyPressed;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            OnWASDPressed?.Invoke(this, new OnWASDPressedEventArgs
            {
                keyPressed = "A"
            });
        }

        if (Input.GetKey(KeyCode.D))
        {
            OnWASDPressed?.Invoke(this, new OnWASDPressedEventArgs
            {
                keyPressed = "D"
            });
        }

        if (Input.GetKey(KeyCode.W))
        {
            OnWASDPressed?.Invoke(this, new OnWASDPressedEventArgs
            {
                keyPressed = "W"
            });
        }

        if (Input.GetKey(KeyCode.S))
        {
            OnWASDPressed?.Invoke(this, new OnWASDPressedEventArgs
            {
                keyPressed = "S"
            });
        }

    }
}
