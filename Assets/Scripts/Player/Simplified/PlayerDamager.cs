using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamager : MonoBehaviour
{
    public bool active;
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
                health.TakeDamage(attackingModule.Damage);
                attackingModule.GainHeat();
            }
        }
    }
}
