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
    public string dodge = "A";
    public string shield = "B";
    public string attack = "X";
    public string sheathe = "Y";
    [Space(10)]
    public string targeting = "RightStickButton";

    Vector2 movementInput;
    Vector2 targetingInput;

    float targetingInputAmount
    {
        get
        {
            return Mathf.Clamp01(Mathf.Abs(targetingInput.x) + Mathf.Abs(targetingInput.y));
        }
    }

    public bool Sheathed { get; private set; }

    bool controlsEnabled;

    private void Update()
    {
        if(!controlsEnabled)
        {
            return;
        }

        // Update Inputs
        movementInput.Set(Input.GetAxis(movementHorizontal), Input.GetAxis(movementVertical));
        targetingInput.Set(Input.GetAxis(targetingHorizontal), Input.GetAxis(targetingVertical));

        // Movement

    }
}
