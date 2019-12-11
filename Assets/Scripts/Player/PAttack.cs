using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PAttack : MonoBehaviour
{
    Animator animator;
    Damager damager;

    private void Start()
    {
        animator = GetComponent<Animator>();
        damager = GetComponentInChildren<Damager>();
    }

    public void EnableDamage()
    {
        damager.CanDamage = true;
    }

    public void DisableDamage()
    {
        damager.CanDamage = false;
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
