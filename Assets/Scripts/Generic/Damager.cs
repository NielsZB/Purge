using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
    [SerializeField] float damage = 5f;
    public bool CanDamage = false;
    private void OnTriggerEnter(Collider other)
    {
        if (CanDamage)
        {
            if (other.TryGetComponent(out Health health))
            {
                health.TakeDamage(damage);
            }
        }
    }
}
