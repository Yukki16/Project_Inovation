using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingObject : MonoBehaviour
{
    [SerializeField] private RingObjectSO ringObjectSO;
    
    public RingObjectSO GetRingObjectSO() { return ringObjectSO; }
}
