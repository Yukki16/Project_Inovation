using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerCharacter;

public class GlidingCharacterCollider : MonoBehaviour
{
    [SerializeField] private PlayerCharacter playerCharacter;
    private void OnTriggerEnter(Collider other)
    {
        if (!playerCharacter.GetPassedRings().Contains(other.transform))
        {
            playerCharacter.AddPassedRing(other.transform);
            playerCharacter.BoostPlayer();
            Debug.Log("Hit");
        }
    }
}
