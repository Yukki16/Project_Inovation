using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate_self : MonoBehaviour
{
    public GameObject item;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        item = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        item.transform.Rotate(0, 0, speed);
    }
}
