using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public static InputController Instance { get; private set; }
    private InputKeys inputKeys;
    public event EventHandler OnIsReady;

    private void Awake()
    {
        Instance = this;
        inputKeys = new InputKeys();
        inputKeys.Player.Enable();
        inputKeys.Player.Interact.performed += Interact_performed;
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnIsReady?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementFromInput()
    {
        Vector2 inputVector = inputKeys.Player.TowerClimbMovement.ReadValue<Vector2>();
        inputVector = inputVector.normalized;
        return inputVector;
    }

    public bool IsReady()
    {
        return inputKeys.Player.Interact.ReadValue<bool>();
    }
}
