using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class GlidingInput : MonoBehaviour
{
    Rigidbody rg;
    private void Start()
    {
       Input.gyro.enabled = true;
        rg = this.GetComponent<Rigidbody>();
    }
    private void Update()
    {
       //if(isLocalPlayer || isClient)
       //{
            ModifyRotation(Input.gyro.attitude);
       // }
        
    }
    private void ModifyRotation(Quaternion qInput)
    {
        Quaternion q = GyroToUnity(qInput);
        transform.eulerAngles = new Vector3(q.eulerAngles.y, -q.eulerAngles.z, 0);
        transform.position += transform.rotation * Vector3.forward * 10 * Time.deltaTime;
    }

    private static Quaternion GyroToUnity(Quaternion q)
    {
        return new Quaternion(q.x, q.y, -q.z, -q.w);
    }
}
