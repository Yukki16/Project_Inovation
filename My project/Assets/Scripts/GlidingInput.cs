using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        ModifyRotation();
    }
    void ModifyRotation()
    {
        //Debug.Log("Hallo");
        //transform.rotation = //Quaternion.Euler(Input.gyro.userAcceleration);
        Quaternion q = GyroToUnity(Input.gyro.attitude);
        //q.y = 0;

        //transform.rotation = q;
        //transform.rotation.Set(q.x,q.y,0,q.w);

        transform.eulerAngles = new Vector3(q.eulerAngles.x, -q.eulerAngles.y, 0);
        //transform.position += transform.rotation * Vector3.forward;
        //rg.MoveRotation(Quaternion.Euler(new Vector3(q.x,q.y,0)));
        
        //Debug.Log(Input.gyro.attitude);
    }

    private static Quaternion GyroToUnity(Quaternion q)
    {
        //Debug.Log(q);
        return new Quaternion(q.x, q.y, -q.z, -q.w);
    }
}
