using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private InputKeys inputKeys;

    private void Awake()
    {
        inputKeys = new InputKeys();
        inputKeys.Player.Enable();
    }

    public Vector2 GetMovementFromInput()
    {
        Vector2 inputVector = inputKeys.Player.TowerClimbMovement.ReadValue<Vector2>();
        inputVector = inputVector.normalized;
        return inputVector;
    }
}
