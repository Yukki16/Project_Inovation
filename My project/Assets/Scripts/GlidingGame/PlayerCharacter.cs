using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour, IPlayer
{
    [SerializeField] private string nickname;
    [SerializeField] private float moveSpeed;
    [SerializeField] private GeneralGameManager.CharacterColors characterColor;

    private float booster = 0.5f;
    private int ringsCompleted;
    private List<Transform> passedRings;

    private void Awake()
    {
        passedRings= new List<Transform>();
    }

    private void Update()
    {
        //if (GlidingGameManager.Instance.GameIsPlaying())
        //{
            MoveForward();
        //}
    }

    private void MoveForward()
    {
        transform.position += Vector3.right * Time.deltaTime * moveSpeed;
    }

    public int GetRingsAmountCompleted()
    {
        return ringsCompleted;
    }

    public List<Transform> GetPassedRings()
    {
        return passedRings;
    }

    public void AddPassedRing(Transform ring)
    {
        passedRings.Add(ring);
        ringsCompleted++;
    }

    public void BoostPlayer()
    {
        moveSpeed += booster;
    }

    public GeneralGameManager.CharacterColors GetCharacterColor()
    {
        return characterColor;
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
