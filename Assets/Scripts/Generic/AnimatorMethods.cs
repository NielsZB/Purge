using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorMethods : MonoBehaviour
{
    [SerializeField] protected string parameter;
    Animator animator;

    protected void SetParameter(string parameter)
    {
        this.parameter = parameter;
    }

    protected void SetBool(bool value)
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        animator.SetBool(parameter, value);
    }

    protected void SetInt(int value)
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        animator.SetInteger(parameter, value);
    }

    protected void SetFloat(float value)
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        animator.SetFloat(parameter, value);
    }

    protected void SetTrigger()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        animator.SetTrigger(parameter);
    }
}
