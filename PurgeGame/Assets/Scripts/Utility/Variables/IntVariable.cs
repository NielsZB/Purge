using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewIntVariable", menuName = "Variables/Int")]
public class IntVariable : ScriptableObject
{

    [SerializeField] int value;

    public int Value { get { return value; } }

    public void Apply(int _value)
    {
        value += _value;
    }

    public void Apply(IntVariable _value)
    {
        value += _value.Value;
    }

    public void Set(int _value)
    {
        value = _value;
    }

    public void Set(IntVariable _value)
    {
        value = _value.Value;
    }
}
