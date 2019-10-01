using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Damagable : MonoBehaviour
{
    public abstract void TakeDamage(float damage);
    public abstract void TakeDamage(float damage, Vector3 direction);
}
