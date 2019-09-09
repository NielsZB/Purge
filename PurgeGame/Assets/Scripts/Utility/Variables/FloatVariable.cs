using UnityEngine;

[CreateAssetMenu(fileName = "NewFloatVariable",menuName = "Variables/Float")]
public class FloatVariable : ScriptableObject
{
    [SerializeField] float value;

    public float Value { get { return value; } }

    public void Apply(float _value)
    {
        value += _value;
    }

    public void Apply(FloatVariable _value)
    {
        value += _value.Value;
    }

    public void Set(float _value)
    {
        value = _value;
    }

    public void Set(FloatVariable _value)
    {
        value = _value.Value;
    }
}
