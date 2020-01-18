using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
[RequireComponent(typeof(CinemachineMixingCamera))]
public class SimpleVolumeMixingCamera : MonoBehaviour
{
    CinemachineMixingCamera mixer;
    WeightVolume volume;
    private void Start()
    {
        mixer = GetComponent<CinemachineMixingCamera>();
        volume = GetComponent<WeightVolume>();
    }

    private void FixedUpdate()
    {
        if (volume.HasTarget)
        {
            mixer.m_Weight0 = volume.Weight;
            mixer.m_Weight1 = 1 - volume.Weight;
        }
        else
        {
            if (mixer.enabled)
            {

                float weight = volume.Weight;

                if (weight == 0f || weight == 1f)
                {
                    mixer.enabled = false;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        mixer.enabled = true;

        float weight = volume.GetWeight();

        mixer.m_Weight0 = weight;
        mixer.m_Weight1 = 1 - weight;
    }
}
