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
                this.transform.parent = target;
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
}
