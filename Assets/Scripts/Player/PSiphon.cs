using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSiphon : MonoBehaviour
{
    [SerializeField] float radius = 1.5f;
    [SerializeField] LayerMask mask;

    Health healthModule;
    PMovement movementModule;
    Collider[] colliders = new Collider[4];
    private void Start()
    {
        healthModule = GetComponent<Health>();
        movementModule = GetComponent<PMovement>();
    }
    public void Life()
    {
        healthModule.GainHealth(GetTargetSiphonHealth());
    }

    public void Energy()
    {
        movementModule.BoostSpeedTemporarily();
        healthModule.GainHealth(GetTargetSiphonHealth());
    }

    float GetTargetSiphonHealth()
    {
        int numberOfTargets = Physics.OverlapSphereNonAlloc(transform.position, radius, colliders, mask);
        if (numberOfTargets > 0)
        {
            for (int i = 0; i < numberOfTargets; i++)
            {
                Health enemyHealth = colliders[i].GetComponent<Health>();

                if (enemyHealth != null)
                {
                    if (enemyHealth.Siphonable)
                    {
                        return enemyHealth.SiphonAmount;
                    }
                }
            }
        }
        return 0f;
    }
}
