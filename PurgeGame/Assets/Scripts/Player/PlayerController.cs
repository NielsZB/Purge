using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerController : MonoBehaviour
{
    #region Serialized Variables

    [System.Serializable]
    struct InputList
    {
        public string verticalLeft;
        public string horizontalLeft;
        public string dodge;
        public string attack;
        public string targeting;
        public string verticalRight;
        public string horizontalRight;

        public InputList(string _vertical, string _horizontal, string _dodge, string _attack, string _targeting, string _verticalRight, string _horizontalRight)
        {
            verticalLeft = _vertical;
            horizontalLeft = _horizontal;
            dodge = _dodge;
            attack = _attack;
            targeting = _targeting;
            verticalRight = _verticalRight;
            horizontalRight = _horizontalRight;
        }
    }

    [SerializeField] InputList inputs = new InputList("Vertical", "Horizontal", "Dodge", "Attack", "Targeting", "VerticalRight", "HorizontalRight");

    #endregion

    #region Private Variables

    PlayerMovement movementModule;
    PlayerTargeting targetingModule;

    bool targeted;

    Vector2 leftStick;
    Vector2 rightStick;

    #endregion

    private void Start()
    {
        movementModule = GetComponent<PlayerMovement>();
        targetingModule = GetComponent<PlayerTargeting>();
    }

    private void Update()
    {
        if (movementModule.InputIsEnabled)
        {
            leftStick.Set(Input.GetAxis(inputs.horizontalLeft), Input.GetAxis(inputs.verticalLeft));
            // MOVING
            movementModule.SetInput(leftStick);


            // DODGING
            if (Input.GetButtonDown(inputs.dodge))
            {
                movementModule.Dodge(leftStick);
            }

            // TOGGLE TARGETING
            if (Input.GetButtonDown(inputs.targeting))
            {
                targetingModule.ToggleTargeting();


                if (targetingModule.IsTargeting)
                {
                    targetingModule.PickTarget();
                }

                movementModule.SetTarget(targetingModule.Target);
            }

            if (targetingModule.IsTargeting)
            {
                rightStick.Set(Input.GetAxis(inputs.horizontalRight), Input.GetAxis(inputs.verticalRight));

                //SELECT TARGET
                if (Mathf.Clamp01(rightStick.magnitude) > 0.5f)
                {
                    targetingModule.PickTarget(rightStick);
                    movementModule.SetTarget(targetingModule.Target);
                    targeted = true;
                }

                //RESET TARGETING INPUT
                if (Mathf.Clamp01(rightStick.magnitude) < 0.2f)
                {

                    targeted = false;
                }
            }
            else
            {
                movementModule.SetTarget(null);
            }
        }
    }
}
