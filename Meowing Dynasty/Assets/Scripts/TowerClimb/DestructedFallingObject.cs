using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructedFallingObject : FallingObject
{
    private float timeToLive = 5;

    protected override void Update()
    {
        base.Update();
        timeToLive -= Time.deltaTime;
        if (timeToLive <= 0)
        {
            DestroyCurrentFallingObject();
        }
    }
}
