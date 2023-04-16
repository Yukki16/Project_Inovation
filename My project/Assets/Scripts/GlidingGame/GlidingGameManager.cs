using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlidingGameManager : MonoBehaviour
{
    [SerializeField] private int gameLengthInMeters;

    public static GlidingGameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public int GetGameLengthInMeters()
    {
        return gameLengthInMeters;
    }
}
