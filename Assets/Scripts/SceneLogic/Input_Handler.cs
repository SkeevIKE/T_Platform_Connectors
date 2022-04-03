using System;
using UnityEngine;

public class Input_Handler : MonoBehaviour
{
    private bool _isHold;

    public event Action<bool> ControlInteractionEvent;   
    public event Action<Vector3> ControlPositonEvent;

    private void Update()
    {
        CheckInput();       
        CheckInputPosition();        
    }

    // Checking position of the control
    private void CheckInputPosition()
    {      
        ControlPositonEvent?.Invoke(Input.mousePosition);
    }

    // Control click tracking
    private void CheckInput()
    {        
        if (Input.GetMouseButtonDown(0))
        {           
            _isHold = true;
            ControlInteractionEvent?.Invoke(_isHold);
        }

        if (Input.GetMouseButtonUp(0))
        {           
            _isHold = false;            
            ControlInteractionEvent?.Invoke(_isHold);
        }        
    }
}
