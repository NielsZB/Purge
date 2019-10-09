using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCheck : MonoBehaviour
{
    public Transform Target { get; private set; }
    public bool HasTarget { get { return Target != null; } }

    public Vector3 enterPoint { get; private set; }
    public Vector3 exitPoint { get; private set; }
    private void OnTriggerEnter(Collider other)
    {
        Target = other.transform;
    }

    private void OnTriggerExit(Collider other)
    {
        Target = null;
    }
}
