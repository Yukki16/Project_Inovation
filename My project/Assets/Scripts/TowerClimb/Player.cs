using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class Player : NetworkBehaviour
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
    public class OnPlayerLeaveArgs
    {
        public ulong clientId;
    }
    public static event EventHandler<OnPlayerLeaveArgs> OnPlayerLeave;

    public enum MovingDirections
    {
        ONLYUP,
        LEFT,
        RIGHT
    }

    #region SPEED_STATS
    [SerializeField] private float moveSpeedSide = 4f;
    
    [SerializeField] private const float DEFAULTACCELERATIONSPEED = 25;
    
    [SerializeField] private float accelerationSpeed = 25;
    
    [SerializeField] private const float MINMOVESPEEDUP = 1f;
    
    [SerializeField] private const float MAXMOVESPEEDUP = 3f;
    
    private float moveSpeedUp;
    #endregion

    [SerializeField] private GameObject playerBody;
    
    [SerializeField] private string nickname;

    #region TIMERS
    private const int ROTATEMOVESPEED = 75;

    private const float SLOWEDTIMERMAX = 8;

    private const float FROZENTIMERMAX = 2;
    
    private const float BOOSTTIMERMAX = 10;
    
    private float frozenTimer;
    
    private float boostTimer;
    
    private float slowedTimer;
    
    private const float HITBYOTHERPLAYERTIMERMAX = 1;
    
    private float hitByOtherPlayerTimer;
    #endregion

    #region BOOLS
    private bool isFrozen;

    private bool isBoosted;

    private bool isSlowed;

    private bool isHitByOtherPlayer;

    #endregion
    private Vector3 hitByOtherPlayerDir;

    #region SCORE
    private float score = 0;
    
    private const int DEFAULTPOINTSINCREASEAMOUNT = 1;
    
    private const int HITPOINTSDECREASE = 5;
    
    private const int HITCOINSPOINTSAMOUNT = 20;
    #endregion
    //private CameraScript _camera;

    [SerializeField] private MovingDirections currentMovingDirection;


    #region GYROSCOPE
    Quaternion offset = Quaternion.identity;
    bool hasGyroScope;
    #endregion


    #region UI
    [SerializeField] GameObject phoneUI;
    public MoveOnUI moveUIScript;
    #endregion

    private void Awake()
    {
        hitByOtherPlayerTimer = HITBYOTHERPLAYERTIMERMAX;
        frozenTimer = FROZENTIMERMAX;
        currentMovingDirection = MovingDirections.ONLYUP;
        moveSpeedUp = MINMOVESPEEDUP;
        accelerationSpeed = DEFAULTACCELERATIONSPEED;

        //moveUIScript = GameObject.FindGameObjectWithTag("PhoneUI").GetComponentInChildren<MoveOnUI>();
    }

    private void Start()
    {
        Input.gyro.enabled = true;
        GameManager.Instance.SlowDownPlayer += Instance_SlowDownPlayer;

        if(!IsOwner)
        {
            phoneUI.SetActive(false);
            GetComponentInChildren<Camera>().enabled = false;
        }

        if(IsServer)
        {
            phoneUI.SetActive(true);
        }

        /*if(SystemInfo.supportsGyroscope)
        {
            hasGyroScope = true;
        }
        else
        {
            hasGyroScope = false;
        }*/
        //GameObject.FindGameObjectWithTag("PhoneUI").gameObject.GetComponent<Canvas>().worldCamera = this.GetComponentInChildren<Camera>();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        List<float> rotationPossibilties = new List<float> { 0, 90, 180, 270 };
        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            if (client.PlayerObject)
            {
                if (client.PlayerObject.GetComponent<Player>() != this)
                {
                    rotationPossibilties.Remove(client.PlayerObject.GetComponent<Player>().transform.rotation.eulerAngles.y);
                }
            }              
        }
        transform.Rotate(0, rotationPossibilties[0], 0);

        if (IsServer)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback += Singleton_OnClientDisconnectCallback;
        }
        
    }

    private void Singleton_OnClientDisconnectCallback(ulong clientId)
    {
       OnPlayerLeave?.Invoke(this, new OnPlayerLeaveArgs { clientId = clientId} );
    }

    private void Instance_SlowDownPlayer(object sender, GameManager.SlowDownPlayerArgs e)
    {
        if (e.player == this)
        {
            SlowDown();
        }
    }

    private void Update()
    {
        if (!IsLocalPlayer)
        {
            this.gameObject.GetComponentInChildren<Camera>().enabled = false;
        }

        if (!IsOwner)
        {
            return;
        }
        

        if (TCMiniGameStateManager.Instance.GameIsPlaying()) 
        {
            if (IsOwner)
            {
                Quaternion inputVector = new Quaternion();
                /*if (!hasGyroScope)
                {
                    inputVector = Quaternion.Euler(InputController.Instance.GetMovementFromInput());
                }
                else
                {*/
                    inputVector = GyroToUnity(offset * Input.gyro.attitude);
                    inputVector.w = -inputVector.w;
                //}
                HandleMovementServerRpc(inputVector);
            }
            //MoveUIClientRpc();
        } 
    }
    private static Quaternion GyroToUnity(Quaternion q)
    {
        return new Quaternion(q.x, q.y, -q.z, -q.w);
    }

    [ServerRpc(RequireOwnership = false)]
    private void HandleMovementServerRpc(Quaternion inputVector)
    {
        //Debug.Log("I am handling movement");
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
            //AddScore(DEFAULTPOINTSINCREASEAMOUNT * Time.deltaTime);
            AccelerateCurrentSpeed();
            HandleBoost();
            HandleSlowedDown();
            HandleSideMovement(inputVector);
            MoveUp();
        }
    }

    private void HandleSideMovement(Quaternion inputVector)
    {
       /* if (!hasGyroScope)
        {
            if (inputVector.eulerAngles.x != 0)
            {
                Vector3 moveDir = new Vector3(0, -inputVector.eulerAngles.x, 0);
                RotatePlayer(moveDir);
            }
            else
            {
                currentMovingDirection = MovingDirections.ONLYUP;
                moveUIScript.RotatePlayerUI(MoveOnUI.RotationDirection.NONE);
            }
        }
        else
        {*/
            if (inputVector.eulerAngles.z != 0)
            {
                Vector3 moveDir = new Vector3(0, inputVector.eulerAngles.z, 0);
                RotatePlayer(moveDir);
            }
            else
            {
                currentMovingDirection = MovingDirections.ONLYUP;
                moveUIScript.RotatePlayerUI(MoveOnUI.RotationDirection.NONE);
            }
        //}
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

    private void ExecutePointUpdateEvent(PointsUpdateArgs.UpdateTypes updateType, int pointAmount)
    {
        PointsUpdated?.Invoke(this, new PointsUpdateArgs
        {
            player = this,
            type = updateType,
            pointAmount = pointAmount
        });
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

    public void HitPlayer(GameObject gameObject)
    {
        if (gameObject.TryGetComponent<DestructableFallingObject>(out DestructableFallingObject fallingItem))
        {
            if (fallingItem.GetObjectType() == DestructableFallingObject.ObjectType.ToAvoid)
            {
                FreezeMovement();
                //DecreaseScore(HITPOINTSDECREASE);
                //ExecutePointUpdateEvent(PointsUpdateArgs.UpdateTypes.POINTDECREASE, HITPOINTSDECREASE);
            }
            else if (fallingItem.GetObjectType() == DestructableFallingObject.ObjectType.SlowDownTopPlayer)
            {
                BoostPlayer();
                GameManager.Instance.SlowDownTopPlayer();
                //ExecutePointUpdateEvent(PointsUpdateArgs.UpdateTypes.POINTINCREASE, HITCOINSPOINTSAMOUNT);
            }
            else if (fallingItem.GetObjectType() == DestructableFallingObject.ObjectType.Boost)
            {
                BoostPlayer();
                //ExecutePointUpdateEvent(PointsUpdateArgs.UpdateTypes.POINTINCREASE, HITCOINSPOINTSAMOUNT);
            }
        }
    }

    public void HitAndRotatePlayer(GameObject gameObject, Vector3 moveDir)
    {
        if (gameObject.TryGetComponent<Player>(out Player otherPlayer))
        {
            isHitByOtherPlayer = true;
            FreezeMovement();
            //DecreaseScore(HITPOINTSDECREASE);
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
            moveUIScript.RotatePlayerUI(MoveOnUI.RotationDirection.RIGHT);
        }
        else
        {
            currentMovingDirection = MovingDirections.LEFT;
            moveUIScript.RotatePlayerUI(MoveOnUI.RotationDirection.LEFT);
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
}
