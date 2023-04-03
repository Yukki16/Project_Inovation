using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public class PointsUpdateArgs
    {
        public enum UpdateTypes
        {
            POINTDECREASE,
            POINTINCREASE
        }

        public UpdateTypes type;
        public Player player;
        public int pointAmount;
    }

    public event EventHandler<PointsUpdateArgs> PointsUpdated;

    public enum MovingDirections
    {
        ONLYUP,
        LEFT,
        RIGHT
    }

    [SerializeField] private float moveSpeedSide = 4f;
    [SerializeField] private float moveSpeedUp = 0.5f;
    [SerializeField] private InputController inputController;
    [SerializeField] private GameObject playerBody;
    [SerializeField] private CameraScript _camera;
    [SerializeField] private string nickname;

    private const int ROTATEMOVESPEED = 75;

    private const float FROZENTIMERMAX = 2;
    private float frozenTimer;
    private bool isFrozen;

    private const float HITBYOTHERPLAYERTIMERMAX = 1;
    private float hitByOtherPlayerTimer;
    private bool isHitByOtherPlayer;
    private Vector3 hitByOtherPlayerDir;

    private float score = 0;
    private const int DEFAULTPOINTSINCREASEAMOUNT = 1;
    private const int HITPOINTSDECREASE = 5;
    private const int HITCOINSPOINTSAMOUNT = 20;

    [SerializeField] private MovingDirections currentMovingDirection;

    private void Awake()
    {
        hitByOtherPlayerTimer = HITBYOTHERPLAYERTIMERMAX;
        frozenTimer = FROZENTIMERMAX;
        currentMovingDirection = MovingDirections.ONLYUP;
    }

    private void Update()
    {
        if (TCMiniGameStateManager.Instance.GameIsPlaying()) 
        {
            HandleMovement();
            _camera.LockCameraAtPlayer(transform);
        } 
    }

    public void HandleMovement()
    {
        if (isHitByOtherPlayer)
        {
            RotatePlayer(hitByOtherPlayerDir != default ? hitByOtherPlayerDir : Vector3.zero);
            hitByOtherPlayerTimer -= Time.deltaTime;
            if (hitByOtherPlayerTimer <= 0)
            {
                isHitByOtherPlayer = false;
                hitByOtherPlayerTimer = HITBYOTHERPLAYERTIMERMAX;
                isFrozen = false;
            }
        }
        else if (isFrozen)
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
            AddScore(DEFAULTPOINTSINCREASEAMOUNT * Time.deltaTime);
            MoveUp();
            Vector2 inputVector = inputController.GetMovementFromInput();
            if (inputVector.x != 0)
            {
                Vector3 moveDir = new Vector3(0, -inputVector.x, 0);
                RotatePlayer(moveDir);           
            }
            else
            {
                currentMovingDirection = MovingDirections.ONLYUP;
            }
            
        }
    }

    private void AddScore(float scoreToAdd)
    {
        score += scoreToAdd;
    }

    private void DecreaseScore(int pointsToDecrease)
    {
        score -= pointsToDecrease;
    }

    private void MoveUp()
    {
        Vector3 moveDir = new Vector3(0, moveSpeedUp, 0);
        transform.position += moveDir * moveSpeedSide * Time.deltaTime;
    }

    private void FreezeMovement()
    {
        isFrozen = true;
    }

    public void HitPlayer(GameObject gameObject)
    {
        if (gameObject.TryGetComponent<DestructableFallingObject>(out DestructableFallingObject fallingItem))
        {
            if (fallingItem.GetObjectType() == DestructableFallingObject.ObjectType.ToAvoid)
            {
                FreezeMovement();
                DecreaseScore(HITPOINTSDECREASE);
                ExecutePointUpdateEvent(PointsUpdateArgs.UpdateTypes.POINTDECREASE, HITPOINTSDECREASE);
            }
            else
            {
                AddScore(HITCOINSPOINTSAMOUNT);
                ExecutePointUpdateEvent(PointsUpdateArgs.UpdateTypes.POINTINCREASE, HITCOINSPOINTSAMOUNT);
            }
        }
    }

    public void HitAndRotatePlayer(GameObject gameObject, Vector3 moveDir)
    {
        if (gameObject.TryGetComponent<Player>(out Player otherPlayer))
        {
            isHitByOtherPlayer = true;
            FreezeMovement();
            DecreaseScore(HITPOINTSDECREASE);
            ExecutePointUpdateEvent(PointsUpdateArgs.UpdateTypes.POINTDECREASE, HITPOINTSDECREASE);
            hitByOtherPlayerDir = moveDir;
        }
    }

    public string GetNickname()
    {
        return nickname == default ? "Unnamed player" : nickname;
    }

    public int GetScore()
    {
        return (int)score;
    }

    public MovingDirections GetCurrentMovingDirection()
    {
        return currentMovingDirection;
    }

    public void RotatePlayer(Vector3 moveDir, int rotateSpeed = ROTATEMOVESPEED)
    {
        if (-moveDir.y > 0)
        {
            currentMovingDirection = MovingDirections.RIGHT;
        }
        else
        {
            currentMovingDirection = MovingDirections.LEFT;
        }
        transform.Rotate(moveDir, rotateSpeed * Time.deltaTime);
    }

    public bool IsHitByOtherPlayer()
    {
        return isHitByOtherPlayer;
    }

    private void ExecutePointUpdateEvent(PointsUpdateArgs.UpdateTypes updateType, int pointAmount)
    {
        PointsUpdated?.Invoke(this,new PointsUpdateArgs
        {
            player = this,
            type = updateType,
            pointAmount = pointAmount
        });
    }
}
