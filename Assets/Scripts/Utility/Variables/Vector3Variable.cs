using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewVecter3Variable", menuName = "Variables/Vecter3")]
public class Vector3Variable : ScriptableObject
{
    [SerializeField] Vector3 value;

    public Vector3 Value { get { return value; } }

    public void SetVector(Vector3 _vector)
    {
        value = _vector;
    }

    /// <summary>
    /// Sets the vector to the position of a transform.
    /// </summary>
    /// <param name="_transform"></param>
    public void SetVector(Transform _transform)
    {
        value = _transform.position;
    }

    public void SetVector(Vector3Variable _vector)
    {
        value = _vector.Value;
    }

    public void SetVector(float _x, float _y, float _z)
    {
        value.Set(_x, _y, _z);
    }
}
