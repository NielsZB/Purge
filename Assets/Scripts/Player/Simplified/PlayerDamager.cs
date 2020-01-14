using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamager : MonoBehaviour
{
    Sword attackingModule;

    private void Start()
    {
        attackingModule = GetComponentInParent<Sword>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Health health))
        {

            health.TakeDamage(attackingModule.Damage);
            attackingModule.GainHeat();
        }
    }


    private void Update()
    {
        if (Input.GetButtonDown("X") && !attackingModule.overheated && !attackingModule.sheathed)
            attack();
    }
    public void attack()
    {
        attackingModule.GainHeat();
    }
}
