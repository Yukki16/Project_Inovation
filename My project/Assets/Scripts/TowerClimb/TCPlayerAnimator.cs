using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TCPlayerAnimator : MonoBehaviour
{
    [SerializeField] private TCPLayer player;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();    
    }

    private void Start()
    {
        animator.SetBool("IsHit", false);
        animator.SetBool("IsMoving", false);
    }

    void Update()
    {
        if (TCMiniGameStateManager.Instance.GameIsPlaying())
        {
            if (!player.IsMoving())
            {
                animator.SetBool("IsHit", true);
                animator.SetBool("IsMoving", false);
            }
            else
            {
                animator.SetBool("IsHit", false);
                animator.SetBool("IsMoving", true);
            }
        }
    }
}
