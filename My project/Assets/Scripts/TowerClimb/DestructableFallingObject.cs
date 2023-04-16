using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableFallingObject : FallingObject
{
    public enum ObjectType
    {
        ToAvoid,
        EarnPoints,
        SlowDownTopPlayer,
        Boost
    }

    [SerializeField] private DestructedFallingObject fracturedObject;
    [SerializeField] private ObjectType objectType;

    private void OnTriggerEnter(Collider other)
    {
        TCPLayer player = other.GetComponentInParent<TCPLayer>();
        if (player)
        {
            if (TCMiniGameStateManager.Instance.GameIsPlaying())
            {
                player.HitPlayer(gameObject);
            }  
            Destroy();
        }
    }

    public void Destroy()
    {
        GameObject newObj = new GameObject("Destroyed Falling Item");
        Transform transformO = Instantiate(fracturedObject.GetFallingObjectSO().prefab, newObj.transform);
        transformO.localPosition = transform.position;
        DestroyCurrentFallingObject();
    }

    public ObjectType GetObjectType()
    {
        return objectType;
    }
}
