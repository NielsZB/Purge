using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
    [SerializeField] float damage;
    [SerializeField] bool active;

    EnemyHealth ownHealth;

    private void Start()
    {
        ownHealth = GetComponentInParent<EnemyHealth>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (active)
        {
            if (other.TryGetComponent(out PlayerHealth playerHealth))
            {
                playerHealth.TakeDamage(damage);
            }
            else if (other.TryGetComponent(out EnemyHealth enemyHealth))
            {
                if (enemyHealth != null)
                {
                    if (enemyHealth == ownHealth)
                        return;

                    enemyHealth.TakeDamage(damage / 7.5f);
                }

                enemyHealth.TakeDamage(damage);
            }
        }
    }
}
