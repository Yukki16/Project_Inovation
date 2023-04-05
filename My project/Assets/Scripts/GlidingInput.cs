using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class GlidingInput : MonoBehaviour
{
    Rigidbody rg;
    Quaternion offset;
    private void Start()
    {
        Input.gyro.enabled = true;
        rg = this.GetComponent<Rigidbody>();
        offset = Quaternion.Euler(90f, 0f, 0f);//* Quaternion.Inverse(GyroToUnity(Input.gyro.attitude));
    }
    private void Update()
    {
       //if(isLocalPlayer || isClient)
        ModifyRotation(Input.gyro.attitude);
    }
    private void ModifyRotation(Quaternion qInput)
    {
        
            if(Input.GetKeyDown(KeyCode.A))
            {
                transform.position += Vector3.forward;
            }
            Quaternion q = offset * GyroToUnity(qInput);
            transform.eulerAngles = new Vector3(q.eulerAngles.x, -q.eulerAngles.y, 0);
        
    }

    public static Quaternion GyroToUnity(Quaternion q)
    {
        //Debug.Log("Got Here");
        //Debug.Log(q);
        return new Quaternion(q.x, q.y, -q.z, -q.w);
    }
}
