using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamager : MonoBehaviour
{
    float damage;
    
    Attacking attackingModule;
    private void Start()
    {
        attackingModule = GetComponentInParent<Attacking>();
        damage = attackingModule.damage;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Health health))
        {
            health.TakeDamage(damage);
            attackingModule.GainHeat();
        }
    }
}
