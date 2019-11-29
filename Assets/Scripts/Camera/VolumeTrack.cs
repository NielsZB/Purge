using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(WeightVolume))]
public class VolumeTrack : MonoBehaviour
{
    
    [SerializeField] CinemachineVirtualCamera cameraDolly = null;

    WeightVolume volume;
    CinemachineTrackedDolly dolly;
    private void Start()
    {
        volume = GetComponent<WeightVolume>();
        dolly = cameraDolly.GetCinemachineComponent<CinemachineTrackedDolly>();
        dolly.m_PositionUnits = CinemachinePathBase.PositionUnits.Normalized;
    }

    private void FixedUpdate()
    {
        if (volume.HasTarget)
        {
            dolly.m_PathPosition = volume.Weight;
        }
        else
        {
            if (cameraDolly.enabled)
            {
                float weight = volume.Weight;

                dolly.m_PathPosition = weight;
                if (weight == 0f)
                {
                    dolly.m_PathPosition = 0f;
                }
                else if (weight == 1f)
                {
                    dolly.m_PathPosition = 1f;
                }
            }
        }
    }
}
