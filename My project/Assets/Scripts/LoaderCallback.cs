using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaderCallback : MonoBehaviour
{
    private float minLoadTimer = 1;

    // Update is called once per frame
    void Update()
    {
        minLoadTimer -= Time.deltaTime;
        if (minLoadTimer <= 0)
        {
            Loader.LoaderCallBack();           
        }
    }
}
