using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    public class RingsCompletedArgs
    {
        public PlayerCharacter character;
    }
    public event EventHandler<RingsCompletedArgs> RingsCompletedUpdated;

    [SerializeField] private Transform spawnPosition;
    [SerializeField] private string nickname;
    private int ringsCompleted;
    private List<Transform> passedRings;

    private void Awake()
    {
        passedRings= new List<Transform>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!passedRings.Contains(other.transform))
        {
            passedRings.Add(other.transform);
            ringsCompleted++;
            RingsCompletedUpdated?.Invoke(this,new RingsCompletedArgs { character = this});
            Debug.Log(ringsCompleted);
        }
    }

    public int GetRingsAmountCompleted()
    {
        return ringsCompleted;
    }
}
