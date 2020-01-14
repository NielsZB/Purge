using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PAttack : MonoBehaviour
{
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void ResetAttack()
    {
        animator.SetBool("Attack", false);
    }
    public void Melee()
    {
        animator.SetBool("Attack", true);
    }
    public void MagicProjectile()
    {

    }
    public void TimeWarpProjectile()
    {

    }
}
