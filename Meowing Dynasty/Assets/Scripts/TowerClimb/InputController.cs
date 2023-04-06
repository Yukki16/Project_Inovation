using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public static InputController Instance { get; private set; }
    private InputKeys inputKeys;

    private void Awake()
    {
        Instance = this;
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
