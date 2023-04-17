using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
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
        Player player = other.GetComponentInParent<Player>();
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
        Transform transformO = Instantiate(fracturedObject.GetFallingObjectSO().prefab, transform.position,transform.rotation);
        transformO.localPosition = transform.position;
        NetworkObject netObj = transformO.GetComponent<NetworkObject>();
        netObj.Spawn(true);
        gameObject.GetComponent<NetworkObject>().Despawn();
    }

    public ObjectType GetObjectType()
    {
        return objectType;
    }
}
