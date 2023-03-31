using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeedSide = 4f;
    [SerializeField] private float moveSpeedUp = 0.5f;
    [SerializeField] private InputController inputController;
    [SerializeField] private GameObject playerBody;
    [SerializeField] private CameraScript _camera;

    private const float FROZENTIMERMAX = 2;
    private float frozenTimer;
    private bool isFrozen;

    private bool isMoving;

    private void Awake()
    {
        frozenTimer = FROZENTIMERMAX;
    }

    private void Update()
    {
        if (TCMiniGameManager.Instance.GameIsPlaying()) 
        {
            HandleMovement();
            _camera.LockCameraAtPlayer(transform);
        } 
    }

    public void HandleMovement()
    {
        if (isFrozen)
        {
            frozenTimer -= Time.deltaTime;
            if (frozenTimer <= 0)
            {
                isFrozen = false;
                frozenTimer = FROZENTIMERMAX;
            }
        }
        else
        {
            MoveUp();
            Vector2 inputVector = inputController.GetMovementFromInput();
            Vector3 moveDir = new Vector3(0, -inputVector.x, 0);
            float rotateSpeed = 75;
            transform.Rotate(moveDir, rotateSpeed * Time.deltaTime);
        }
    }

    public bool IsMoving()
    {
        return isMoving;
    }

    private void MoveUp()
    {
        Vector3 moveDir = new Vector3(0, moveSpeedUp, 0);
        transform.position += moveDir * moveSpeedSide * Time.deltaTime;

        isMoving = moveDir != Vector3.zero;
    }

    public void FreezeMovement()
    {
        isFrozen = true;
    }
}
