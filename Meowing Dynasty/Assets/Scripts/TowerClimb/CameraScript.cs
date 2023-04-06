using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public void LockCameraAtPlayer(Transform player)
    {
        transform.position = new Vector3(0, player.position.y, 0);
        transform.rotation = player.rotation;
    }
}
