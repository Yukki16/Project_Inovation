using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FollowPlayer : MonoBehaviour
{
    public Transform target;

    private void Start()
    {
        switch(SceneManager.GetActiveScene().name)
        {
            case "GlidingGame":
                this.transform.gameObject.AddComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY |
                                               RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX |
                                               RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
                this.GetComponent<Rigidbody>().useGravity = false;
                break;
            case "TowerClimb":
                this.transform.gameObject.AddComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX |
                                               RigidbodyConstraints.FreezeRotationZ;
                this.GetComponent<Rigidbody>().useGravity = false;
                //this.transform.position = new Vector3(Mathf.Cos(target.rotation.eulerAngles.y) * 30, target.position.y + 6, Mathf.Sin(target.rotation.eulerAngles.y) * 30);
                this.transform.parent = target;
                
                this.transform.LookAt(target);
                //this.transform.rotation.SetEulerRotation(-target.transform.rotation.eulerAngles);
                break;
        }
        /*if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("GlidingGame"))
        {
           this.transform.gameObject.AddComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY |
                                               RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX |
                                               RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        }*/
        //this.transform.LookAt(target);
    }
    void Update()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "GlidingGame":
                Gliding();
                break;
            case "TowerClimb":
                TowerClimbMoving();
                break;
        }
        
    }
    
    private void Gliding()
    {
        if (target != null)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, target.position.z - 10);
        }
        else
        {
            Destroy(this);
        }
    }

    private void TowerClimbMoving()
    {
        if (target != null)
        {
            this.GetComponent<Rigidbody>().MovePosition(target.transform.position);
        }
        else
        {
            Destroy(this);
        }
    }
}
