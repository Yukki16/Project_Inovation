using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class TCPLayer : MonoBehaviour, IPlayer
{
    public enum MovingDirections
    {
        ONLYUP,
        LEFT,
        RIGHT
    }

    [SerializeField] private float moveSpeedSide = 4f;
    [SerializeField] private const float DEFAULTACCELERATIONSPEED = 25;
    [SerializeField] private float accelerationSpeed = 25;
    [SerializeField] private const float MINMOVESPEEDUP = 1f;
    [SerializeField] private const float MAXMOVESPEEDUP = 3f;
    [SerializeField] private GameObject playerBody;
    [SerializeField] private string nickname;
    [SerializeField] private MovingDirections currentMovingDirection;
    [SerializeField] private GeneralGameManager.CharacterColors characterColor;

    private const int ROTATEMOVESPEED = 75;
    private float moveSpeedUp;

    private const float FROZENTIMERMAX = 2;
    private float frozenTimer;
    private bool isFrozen;

    private const float BOOSTTIMERMAX = 10;
    private float boostTimer;
    private bool isBoosted;

    private const float SLOWEDTIMERMAX = 8;
    private float slowedTimer;
    private bool isSlowed;

    private const float HITBYOTHERPLAYERTIMERMAX = 1;
    private float hitByOtherPlayerTimer;
    private bool isHitByOtherPlayer;
    private Vector3 hitByOtherPlayerDir;

    private void Awake()
    {
        hitByOtherPlayerTimer = HITBYOTHERPLAYERTIMERMAX;
        frozenTimer = FROZENTIMERMAX;
        currentMovingDirection = MovingDirections.ONLYUP;
        moveSpeedUp = MINMOVESPEEDUP;
        accelerationSpeed = DEFAULTACCELERATIONSPEED;
    }

    private void Update()
    {
        if (TCMiniGameStateManager.Instance.GameIsPlaying())
        {
            HandleMovement();
        }
    }

    public GeneralGameManager.CharacterColors GetCharacterColor()
    {
        return characterColor;
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
            MoveUp();
            AccelerateCurrentSpeed();
            HandleBoost();
            HandleSlowedDown();
            //HandleSideMovement(inputVector);
        }
    }

    public void HandleSideMovement(Vector2 inputVector)
    {
        if (!isFrozen && !isHitByOtherPlayer)
        {
            if (inputVector.x != 0)
            {
                Vector3 moveDir = new Vector3(0, -inputVector.x, 0);
                RotatePlayer(moveDir, 100);
            }
            else
            {
                currentMovingDirection = MovingDirections.ONLYUP;
            }
        }
        
    }

    //private void AddScore(float scoreToAdd)
    //{
    //    score += scoreToAdd;
    //}

    //private void DecreaseScore(int pointsToDecrease)
    //{
    //    score -= pointsToDecrease;
    //}

    public void HitPlayer(GameObject gameObject)
    {
        if (gameObject.TryGetComponent<DestructableFallingObject>(out DestructableFallingObject fallingItem))
        {
            if (fallingItem.GetObjectType() == DestructableFallingObject.ObjectType.ToAvoid)
            {
                FreezeMovement();
            }
            else if (fallingItem.GetObjectType() == DestructableFallingObject.ObjectType.SlowDownTopPlayer)
            {
                BoostPlayer();
                GameManager.Instance.SlowDownTopPlayer();
            }
            else if (fallingItem.GetObjectType() == DestructableFallingObject.ObjectType.Boost)
            {
                BoostPlayer();
            }
        }
    }

    public void HitAndRotatePlayer(GameObject gameObject, Vector3 moveDir)
    {
        if (gameObject.TryGetComponent<TCPLayer>(out TCPLayer otherPlayer))
        {
            isHitByOtherPlayer = true;
            hitByOtherPlayerDir = moveDir;
        }
    }

    private void MoveUp()
    {
        Vector3 moveDir = new Vector3(0, moveSpeedUp, 0);
        transform.position += moveDir * moveSpeedSide * Time.deltaTime;
    }

    private void FreezeMovement()
    {
        isFrozen = true;
        ResetSpeed();
    }

    private void HandleBoost()
    {
        if (isBoosted)
        {
            boostTimer -= Time.deltaTime;
            if (boostTimer <= 0)
            {
                ResetBoost();
            }
        }
    }

    private void ResetBoost()
    {
        isBoosted = false;
        accelerationSpeed = DEFAULTACCELERATIONSPEED;
        moveSpeedUp = Mathf.Max(moveSpeedUp - 0.5f, MINMOVESPEEDUP);
    }

    private void BoostPlayer()
    {
        if (!isBoosted && !isSlowed)
        {
            accelerationSpeed *= 0.25f;
            boostTimer = BOOSTTIMERMAX;
            isBoosted = true;
        }
    }

    private void AccelerateCurrentSpeed()
    {
        if (!isSlowed)
        {
            if (moveSpeedUp <= MAXMOVESPEEDUP)
            {
                moveSpeedUp += Time.deltaTime / accelerationSpeed;
            }
            else
            {
                moveSpeedUp = MAXMOVESPEEDUP;
            }
        }
    }

    private void ResetSpeed()
    {
        moveSpeedUp = MINMOVESPEEDUP;
    }

    private void HandleSlowedDown()
    {
        if (isSlowed)
        {
            slowedTimer -= Time.deltaTime;
            if (slowedTimer <= 0)
            {
                isSlowed = false;
            }
        }
    }

    private void SlowDown()
    {
        isSlowed = true;
        slowedTimer = SLOWEDTIMERMAX;
        moveSpeedUp = Mathf.Max(moveSpeedUp - 0.5f, MINMOVESPEEDUP);
        if (isBoosted)
        {
            ResetBoost();
        }
    }

    //public void HitPlayer(GameObject gameObject)
    //{
    //    if (gameObject.TryGetComponent<DestructableFallingObject>(out DestructableFallingObject fallingItem))
    //    {
    //        if (fallingItem.GetObjectType() == DestructableFallingObject.ObjectType.ToAvoid)
    //        {
    //            FreezeMovement();
    //            //DecreaseScore(HITPOINTSDECREASE);
    //            //ExecutePointUpdateEvent(PointsUpdateArgs.UpdateTypes.POINTDECREASE, HITPOINTSDECREASE);
    //        }
    //        else if (fallingItem.GetObjectType() == DestructableFallingObject.ObjectType.SlowDownTopPlayer)
    //        {
    //            BoostPlayer();
    //            GameManager.Instance.SlowDownTopPlayer();
    //            //ExecutePointUpdateEvent(PointsUpdateArgs.UpdateTypes.POINTINCREASE, HITCOINSPOINTSAMOUNT);
    //        }
    //        else if (fallingItem.GetObjectType() == DestructableFallingObject.ObjectType.Boost)
    //        {
    //            BoostPlayer();
    //            //ExecutePointUpdateEvent(PointsUpdateArgs.UpdateTypes.POINTINCREASE, HITCOINSPOINTSAMOUNT);
    //        }
    //    }
    //}

    //public void HitAndRotatePlayer(GameObject gameObject, Vector3 moveDir)
    //{
    //    if (gameObject.TryGetComponent<Player>(out Player otherPlayer))
    //    {
    //        isHitByOtherPlayer = true;
    //        FreezeMovement();
    //        //DecreaseScore(HITPOINTSDECREASE);
    //        ExecutePointUpdateEvent(PointsUpdateArgs.UpdateTypes.POINTDECREASE, HITPOINTSDECREASE);
    //        hitByOtherPlayerDir = moveDir;
    //    }
    //}

    public string GetNickname()
    {
        return nickname == default ? "Unnamed player" : nickname;
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

    public Transform GetPlayerBody()
    {
        return playerBody.transform;
    }

    public bool IsMoving()
    {
        return !isFrozen;
    }
}
