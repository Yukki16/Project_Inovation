using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] Transform target;
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, target.position.z - 10);
    }
}
