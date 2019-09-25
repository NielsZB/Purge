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
        public string HorizontalMovement;
        public string VerticalMovement;
        [Space(10)]
        public string RightHorizontal;
        public string RightVertical;
        [Space(10)]
        public string LeftHandMagic;
        public string RightHandMagic;
        [Space(10)]
        public string Dodge;
        public string Parry;
        public string Attack;
        public string Heal;
        [Space(10)]
        public string Targeting;
    }

    [SerializeField] InputList inputs;

    [SerializeField] bool showDebug;
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
            leftStick.Set(Input.GetAxis(inputs.HorizontalMovement), Input.GetAxis(inputs.VerticalMovement));
            // MOVING
            movementModule.SetInput(leftStick);


            // DODGING
            if (Input.GetButtonDown(inputs.Dodge))
            {
                movementModule.Dodge(leftStick);
            }

            // TOGGLE TARGETING
            if (Input.GetButtonDown(inputs.Targeting))
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
                rightStick.Set(Input.GetAxis(inputs.RightHorizontal), Input.GetAxis(inputs.RightVertical));

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
