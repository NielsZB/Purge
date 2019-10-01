using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct InputList
{
    public string MovementHorizontal;
    public string MovementVertical;
    [Space(10)]
    public string TargetingHorizontal;
    public string TargetingVertical;
    [Space(10)]
    public string LeftHand;
    public string RightHand;
    [Space(10)]
    public string Dodge;
    public string Shield;
    public string Attack;
    public string Heal;
    [Space(10)]
    public string Targeting;

    public InputList(string movementHorizontal, string movementVertical, string targetingHorizontal, string targetingVertical, string leftHand, string rightHand, string dodge, string parry, string attack, string heal, string targeting)
    {
        MovementHorizontal = movementHorizontal;
        MovementVertical = movementVertical;
        TargetingHorizontal = targetingHorizontal;
        TargetingVertical = targetingVertical;
        LeftHand = leftHand;
        RightHand = rightHand;
        Dodge = dodge;
        Shield = parry;
        Attack = attack;
        Heal = heal;
        Targeting = targeting;

    }
}
public class PController : MonoBehaviour
{
    [SerializeField] InputList inputs = new InputList();

    PMovement movementModule;
    PTargeting targetingModule;
    Health healthModule;
    PWard wardModule;
    PAttack attackModule;
    PSiphon siphonModule;

    Vector2 movementInput;
    Vector2 targetingInput;
    float targetingInputAmount
    {
        get
        {
            return Mathf.Clamp01(Mathf.Abs(targetingInput.x) + Mathf.Abs(targetingInput.y));
        }
    }
    bool readyToChangeTarget;
    bool leftHand;
    bool rightHand;
    private void Start()
    {
        movementModule = GetComponent<PMovement>();
        targetingModule = GetComponent<PTargeting>();
        healthModule = GetComponent<Health>();
        wardModule = GetComponent<PWard>();
        attackModule = GetComponent<PAttack>();
        siphonModule = GetComponent<PSiphon>();
    }

    private void Update()
    {
        // Death Check;
        if (healthModule.Dead)
        {
            return;
        }

        // Update inputs
        movementInput.Set(Input.GetAxis(inputs.MovementHorizontal), Input.GetAxis(inputs.MovementVertical));

        targetingInput.Set(Input.GetAxis(inputs.TargetingHorizontal), Input.GetAxis(inputs.TargetingVertical));

        if (Input.GetButtonDown(inputs.LeftHand))
        {
            leftHand = true;
        }
        else if (Input.GetButtonUp(inputs.LeftHand))
        {
            leftHand = false;
        }

        if (Input.GetButtonDown(inputs.RightHand))
        {
            rightHand = true;
        }
        else if (Input.GetButtonUp(inputs.RightHand))
        {
            rightHand = false;
        }

        // Movement
        movementModule.Move(movementInput);



        // Dodge
        if (Input.GetButtonDown(inputs.Dodge))
        {
            if (leftHand)
            {
                if (rightHand)
                {
                    // Both hands
                    movementModule.Move(movementInput, AbilityTypes.EnergizedGlimmer);
                    Debug.Log("Energized Swift Glimmer");
                }
                else
                {
                    // One hand
                    movementModule.Move(movementInput, AbilityTypes.Glimmer);
                    Debug.Log("Swift Glimmer");
                }
            }
            else
            {
                // No hands
                movementModule.Move(movementInput, AbilityTypes.Dodge);
                Debug.Log("Dodge");
            }
        }

        // Warp
        else if (Input.GetButtonDown(inputs.Shield))
        {
            if (leftHand)
            {
                if (rightHand)
                {
                    // Both hands
                    wardModule.Burst();
                    Debug.Log("Ward burst");
                }
                else
                {
                    // One hand
                    wardModule.Ward();
                    Debug.Log("Ward");
                }
            }
        }

        // Attack
        else if (Input.GetButtonDown(inputs.Attack))
        {
            if (leftHand)
            {
                if (rightHand)
                {
                    // Both hands
                    attackModule.TimeWarpProjectile();
                    Debug.Log("Time-warp projectile");
                }
                else
                {
                    // One hand
                    attackModule.MagicProjectile();
                    Debug.Log("Magic missile");
                }
            }
            else
            {
                // No hands
                attackModule.Melee();
                Debug.Log("Attack");
            }
        }

        // Heal
        else if (Input.GetButtonDown(inputs.Heal))
        {
            if (leftHand)
            {
                if (rightHand)
                {
                    // Both hands
                    siphonModule.Energy();
                    Debug.Log("Enegy Siphon");
                }
                else
                {
                    // One hand
                    siphonModule.Life();
                    Debug.Log("Life Siphon");
                }
            }
        }



        // Target
        if (Input.GetButtonDown(inputs.Targeting))
        {
            if (targetingModule.Target == null)
            {
                targetingModule.GetTarget();
            }
            else
            {
                targetingModule.StopTargeting();
            }
        }

        // Change Target
        if (targetingModule.IsEnabled)
        {
            if (targetingInputAmount > 0.75f && readyToChangeTarget)
            {
                readyToChangeTarget = false;
                targetingModule.GetTarget(targetingInput);
            }
            else if (targetingInputAmount < 0.125f)
            {
                readyToChangeTarget = true;
            }
        }
    }
}
