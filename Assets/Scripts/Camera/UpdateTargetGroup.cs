using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class UpdateTargetGroup : MonoBehaviour
{
    CinemachineTargetGroup targetGroup;
    [SerializeField] PlayerTargeting playerTargeting;

    private void Awake()
    {
        targetGroup = GetComponent<CinemachineTargetGroup>();
    }

    private void FixedUpdate()
    {
        if(playerTargeting.IsTargeting)
        {
            if(playerTargeting.Target != null)
            {
                targetGroup.m_Targets[1].target = playerTargeting.Target;
            }
        }
    }
}
