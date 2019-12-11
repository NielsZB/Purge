using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Behavior : ScriptableObject
{
    public abstract Vector3 CalculatePreferredPosition();
}
