using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] float health;

    public float currentHealth { get; private set; }
    public void TakeDamage(float amount)
    {

    }
}
