using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamager : MonoBehaviour
{
    public bool active;
    public bool swordAttack;
    Sword attackingModule;

    private void Start()
    {
        attackingModule = GetComponentInParent<Sword>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (active)
        {
            if (other.TryGetComponent(out EnemyHealth health))
            {
                if(swordAttack)
                {
                health.TakeDamage(attackingModule.Damage);
                attackingModule.GainHeat();
                }
                else
                {
                    health.TakeDamage(attackingModule.StandardDamage);

                }
            }
        }
    }
}
