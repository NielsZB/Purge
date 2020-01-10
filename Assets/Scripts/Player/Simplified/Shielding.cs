using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shielding : MonoBehaviour
{
    [SerializeField] Animator animator = null;

    public void Activate()
    {
        animator.SetTrigger("Shield");
    }
}
