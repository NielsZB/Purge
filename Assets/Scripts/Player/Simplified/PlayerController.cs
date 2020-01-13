using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public string movementHorizontal = "LeftHorizontal";
    public string movementVertical = "LeftVertical";
    [Space(10)]
    public string targetingHorizontal = "LeftVertical";
    public string targetingVertical = "RightVertical";
    [Space(10)]
    public string leftHand = "LB";
    public string rightHand = "RB";
    [Space(10)]
    public string dash = "A";
    public string shield = "B";
    public string attack = "X";
    public string sheathe = "Y";
    [Space(10)]
    public string targeting = "RightStickButton";

    Vector2 movementInput;
    Vector2 targetingInput;

    bool readyToAttack = true;

    float targetingInputAmount
    {
        get
        {
            return Mathf.Clamp01(Mathf.Abs(targetingInput.x) + Mathf.Abs(targetingInput.y));
        }
    }

    public bool Sheathed { get; private set; }

    bool controlsEnabled = true;

    Movement movementModule;
    Targeting targetingModule;
    Attacking attackingModule;
    Animator animatorModule;
    private void Start()
    {
        movementModule = GetComponent<Movement>();
        targetingModule = GetComponent<Targeting>();
        attackingModule = GetComponent<Attacking>();
        animatorModule = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!controlsEnabled)
            return;

        // Update Inputs
        movementInput.Set(Input.GetAxis(movementHorizontal), Input.GetAxis(movementVertical));
        targetingInput.Set(Input.GetAxis(targetingHorizontal), Input.GetAxis(targetingVertical));

        // Movement and Dodge
        movementModule.Move(movementInput, Input.GetButtonDown(dash));
        animatorModule.SetFloat("Movement", movementModule.actualMovementSpeedNormalized);

        // Shield, Dash, Attack and Sheathe sword.
        if (Input.GetButtonDown(shield))
        {
            movementModule.ChangeSpeeds();
            animatorModule.ResetTrigger("Shield");
            animatorModule.SetTrigger("Shield");
        }
        else if (Input.GetButtonDown(dash))
        {
            animatorModule.ResetTrigger("Dash");
            animatorModule.SetTrigger("Dash");
        }
        else if (Input.GetButtonDown(attack))
        {
            int attacknumber = animatorModule.GetInteger("AttackNumber");
            movementModule.ChangeSpeeds();
            if (animatorModule.GetCurrentAnimatorStateInfo(0).IsName("Movement"))
            {
                animatorModule.ResetTrigger("Attack");
                animatorModule.SetTrigger("Attack");
                animatorModule.SetInteger("AttackNumber", 1);
            }
            else if (animatorModule.GetCurrentAnimatorStateInfo(0).IsName("Attack_1"))
            {
                animatorModule.ResetTrigger("Attack");
                animatorModule.SetTrigger("Attack");
                animatorModule.SetInteger("AttackNumber", 2);
            }
            else if (animatorModule.GetCurrentAnimatorStateInfo(0).IsName("Attack_2"))
            {
                animatorModule.ResetTrigger("Attack");
                animatorModule.SetTrigger("Attack");
                animatorModule.SetInteger("AttackNumber", 3);
            }
        }
        else if (Input.GetButtonDown(sheathe))
        {
            if (attackingModule.sheathed)
            {
                if(attackingModule.overheated)
                {
                    return;
                }
                attackingModule.Unsheathe();
                animatorModule.SetBool("Sheathed", false);
            }
            else
            {
                attackingModule.Sheathe();
                animatorModule.SetBool("Sheathed", true);
            }
        }
    }

    public void EnableControls()
    {
        controlsEnabled = true;
        movementModule.enabled = true;
        targetingModule.enabled = true;
        movementModule.EnableMovement();
    }

    public void DisableControls()
    {
        controlsEnabled = false;
        movementModule.DisableMovement();
        movementModule.enabled = false;
        targetingModule.enabled = false;
    }

    public void ResetAttackCombo()
    {
        animatorModule.SetInteger("AttackNumber", 3);
        animatorModule.ResetTrigger("Attack");
    }
}
