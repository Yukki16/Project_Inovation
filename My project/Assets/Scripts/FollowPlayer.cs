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
        Gliding();
    }

    private void Gliding()
    {
        if (target != null)
        {
            transform.position = new Vector3(target.position.x, target.position.y, target.position.z - 20);
        }
        else
        {
            Destroy(this);
        }
    }
}
