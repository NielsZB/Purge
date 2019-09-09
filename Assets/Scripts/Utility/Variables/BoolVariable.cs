using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBoolVariable", menuName = "Variables/Bool")]
public class BoolVariable : ScriptableObject
{

    [SerializeField] bool state;

    public bool State { get { return state; } }

    public void SetState(bool _state)
    {
        state = _state;
    }
 
}
