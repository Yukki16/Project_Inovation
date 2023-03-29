using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FollowPlayer : MonoBehaviour
{
    public Transform target;

    private void Start()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("GlidingGame"))
        {
           this.transform.gameObject.AddComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY |
                                               RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX |
                                               RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        }
    }
    void Update()
    {
        if(SceneManager.GetActiveScene() == SceneManager.GetSceneByName("GlidingGame"))
        Gliding();
    }

    private void Gliding()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, target.position.z - 10);
    }
}
