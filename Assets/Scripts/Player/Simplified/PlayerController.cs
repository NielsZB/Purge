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
    public string ward = "B";
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

    PlayerHealth healthModule;
    Shielding wardModule;
    Movement movementModule;
    Targeting targetingModule;
    Sword attackingModule;
    Animator animatorModule;
    private void Start()
    {
        healthModule = GetComponent<PlayerHealth>();
        wardModule = GetComponent<Shielding>();
        movementModule = GetComponent<Movement>();
        targetingModule = GetComponent<Targeting>();
        attackingModule = GetComponent<Sword>();
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
        if (Input.GetButtonDown(ward))
        {
            wardModule.Activate();
        }
        else if (Input.GetButtonDown(dash))
        {
            animatorModule.ResetTrigger("Dash");
            animatorModule.SetTrigger("Dash");
        }
        else if (Input.GetButtonDown(attack))
        {
            if (attackingModule.sheathed)
                return;
            int attacknumber = animatorModule.GetInteger("AttackNumber");
            movementModule.ChangeSpeeds();
            if (animatorModule.GetCurrentAnimatorStateInfo(0).IsName("Movement With Sword"))
            {
                animatorModule.ResetTrigger("Attack");
                animatorModule.SetTrigger("Attack");
                animatorModule.SetInteger("AttackNumber", 1);
            }
            else if (animatorModule.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
            {
                animatorModule.ResetTrigger("Attack");
                animatorModule.SetTrigger("Attack");
                animatorModule.SetInteger("AttackNumber", 2);
            }
            else if (animatorModule.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
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
                if (attackingModule.overheated)
                {
                    return;
                }
                attackingModule.Unsheathe();
                animatorModule.SetBool("Drawn", true);
            }
            else
            {
                attackingModule.Sheathe();
                animatorModule.SetBool("Drawn", false);
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
