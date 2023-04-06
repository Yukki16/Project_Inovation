using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingSpawner : MonoBehaviour
{
    public static RingSpawner Instance { get; private set; }

    [SerializeField] private RingObject ringObject;
    [SerializeField] private int distanceBetweenRings;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    { 
        int previousRingX = 0;
        for (int i = 0; i < CalculateAmountOfRings(); i++)
        {
            Transform newRing = Instantiate(ringObject.GetRingObjectSO().prefab, transform);
            int randomXPos = previousRingX < 0 ? Random.Range(previousRingX, previousRingX + 51) : Random.Range(previousRingX - 51, previousRingX);
            newRing.localPosition = new Vector3(randomXPos, Random.Range(75, 120), i * distanceBetweenRings);
            previousRingX = (int)newRing.localPosition.x;
        }
    }

    public int CalculateAmountOfRings()
    {
        int gameLength = GlidingGameManager.Instance.GetGameLengthInMeters();
        return (int)Mathf.Floor(gameLength / distanceBetweenRings);
    }
}
