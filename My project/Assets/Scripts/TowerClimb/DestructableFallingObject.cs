using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableFallingObject : FallingObject
{
    public enum ObjectType
    {
        ToAvoid,
        EarnPoints
    }

    [SerializeField] private DestructedFallingObject facturedObject;
    [SerializeField] private ObjectType objectType;

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player)
        {
            Debug.Log(TCMiniGameStateManager.Instance.currentGameState == TCMiniGameStateManager.GameState.PLAYING);
            if (TCMiniGameStateManager.Instance.currentGameState == TCMiniGameStateManager.GameState.PLAYING)
            {
                player.HitPlayer(gameObject);
            }  
            Destroy();
        }
    }

    public void Destroy()
    {
        GameObject newObj = new GameObject("Destroyed Falling Item");
        Transform transformO = Instantiate(facturedObject.GetFallingObjectSO().prefab, newObj.transform);
        transformO.localPosition = transform.position;
        Destroy(gameObject.transform.parent.gameObject);
    }

    public ObjectType GetObjectType()
    {
        return objectType;
    }
}
