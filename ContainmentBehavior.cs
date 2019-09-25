using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Behavior/Containment")]
public class ContainmentBehavior : SwarmBehavior
{

    [SerializeField] Vector3 center = Vector3.zero;
    [SerializeField] float radius = 15f;


}
