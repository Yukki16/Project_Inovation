using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        TCPLayer thisPlayer = GetComponentInParent<TCPLayer>();
        if (thisPlayer)
        {
            if (!thisPlayer.IsHitByOtherPlayer())
            {
                TCPLayer otherPlayer = other.GetComponentInParent<TCPLayer>();
                if (otherPlayer)
                {
                    if (thisPlayer.GetCurrentMovingDirection() == TCPLayer.MovingDirections.LEFT)
                    {
                        if (otherPlayer.GetCurrentMovingDirection() == TCPLayer.MovingDirections.ONLYUP)
                        {
                            Vector3 moveDir = new Vector3(0, 1000, 0);
                            otherPlayer.HitAndRotatePlayer(thisPlayer.gameObject, moveDir);
                        }
                        else if (otherPlayer.GetCurrentMovingDirection() == TCPLayer.MovingDirections.RIGHT)
                        {
                            Vector3 moveDirLeft = new Vector3(0, 1000, 0);
                            otherPlayer.HitAndRotatePlayer(thisPlayer.gameObject, moveDirLeft);
                            Vector3 moveDirRight = new Vector3(0, -1000, 0);
                            thisPlayer.HitAndRotatePlayer(otherPlayer.gameObject, moveDirRight);
                        }
                    }
                    else if (thisPlayer.GetCurrentMovingDirection() == TCPLayer.MovingDirections.RIGHT)
                    {
                        if (otherPlayer.GetCurrentMovingDirection() == TCPLayer.MovingDirections.ONLYUP)
                        {
                            Vector3 moveDir = new Vector3(0, -1000, 0);
                            otherPlayer.HitAndRotatePlayer(thisPlayer.gameObject, moveDir);
                        }
                        else if (otherPlayer.GetCurrentMovingDirection() == TCPLayer.MovingDirections.LEFT)
                        {
                            Vector3 moveDirLeft = new Vector3(0, 1000, 0);
                            thisPlayer.HitAndRotatePlayer(otherPlayer.gameObject, moveDirLeft);
                            Vector3 moveDirRight = new Vector3(0, -1000, 0);
                            otherPlayer.HitAndRotatePlayer(thisPlayer.gameObject, moveDirRight);
                        }
                    }
                    else
                    {
                        if (otherPlayer.GetCurrentMovingDirection() == TCPLayer.MovingDirections.ONLYUP)
                        {
                            Vector3 moveDir = new Vector3(0, -500, 0);
                            otherPlayer.HitAndRotatePlayer(thisPlayer.gameObject, moveDir);
                        }
                    }
                }
            }
        }
        else
        {
            Debug.LogError("Error while getting PlayerComponent of current playerbody gameobject.");
        }
    }
}