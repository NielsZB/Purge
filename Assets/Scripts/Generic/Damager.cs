using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
    [SerializeField] float damage = 5f;
    public bool CanDamage = false;
    public bool HurtsPlayerOnly;
    private void OnTriggerEnter(Collider other)
    {
        if (CanDamage)
        {
            if (HurtsPlayerOnly)
            {
                if (other.TryGetComponent(out PHealth health))
                {
                    Debug.Log(other.gameObject,gameObject);
                    health.TakeDamage(damage);
                }
            }
            else
            {
                if (other.TryGetComponent(out Health health))
                {
                    Debug.Log(other.gameObject,gameObject);
                    health.TakeDamage(damage);
                }
            }
        }
    }
}
