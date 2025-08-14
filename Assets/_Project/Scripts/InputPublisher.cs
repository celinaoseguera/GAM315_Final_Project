using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InputPublisher : MonoBehaviour
{

    public event EventHandler OnSpacePressed;
    public event EventHandler OnEPressed;

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnSpacePressed?.Invoke(this, EventArgs.Empty);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            OnEPressed?.Invoke(this, EventArgs.Empty);
        }

    }
}
