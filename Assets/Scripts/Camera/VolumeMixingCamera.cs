using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(WeightVolume), typeof(CinemachineMixingCamera))]
public class VolumeMixingCamera : MonoBehaviour
{
    WeightVolume volume;
    CinemachineBrain brain;
    CinemachineMixingCamera mixer;
    [SerializeField] CinemachineVirtualCameraBase cameraA = null;
    [SerializeField] CinemachineVirtualCameraBase cameraB = null;
    [SerializeField] bool modifiable = true;

    private void Start()
    {
        volume = GetComponent<WeightVolume>();
        brain = Camera.main.GetComponent<CinemachineBrain>();
        mixer = GetComponent<CinemachineMixingCamera>();
        mixer.Priority = 99;
        mixer.enabled = false;
    }

    private void FixedUpdate()
    {
        if (volume.HasTarget)
        {
            mixer.SetWeight(cameraA, 1f - volume.Weight);
            mixer.SetWeight(cameraB, volume.Weight);
        }
        else
        {
            if (mixer.enabled)
            {
                float weight = volume.Weight;

                if (weight == 0f)
                {
                    cameraA.transform.SetParent(null, true);
                    if(!modifiable)
                    {
                        cameraA.enabled = false;
                    }
                }
                else if (weight == 1f)
                {
                    cameraB.transform.SetParent(null, true);
                    if (!modifiable)
                    {
                        cameraB.enabled = false;
                    }
                }
                var x = mixer.ChildCameras;
                mixer.enabled = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        mixer.enabled = true;
        if ((CinemachineVirtualCameraBase)brain.ActiveVirtualCamera == mixer)
            return;


        float weight = volume.GetWeight();

        cameraB.transform.SetParent(mixer.transform, true);
        cameraB.enabled = true;

        if (modifiable)
        {

            if (weight < 0.5f)
            {
                cameraA = (CinemachineVirtualCameraBase)brain.ActiveVirtualCamera;
                cameraA.transform.SetParent(mixer.transform, true);
                cameraA.enabled = true;
            }
            else
            {
                cameraB = (CinemachineVirtualCameraBase)brain.ActiveVirtualCamera;
                cameraB.transform.SetParent(mixer.transform, true);
                cameraB.enabled = true;
            }
        }
        else
        {
            cameraA.transform.SetParent(mixer.transform, true);
            cameraA.enabled = true;
        }

        var x = mixer.ChildCameras;

        mixer.SetWeight(cameraA, 1f - weight);
        mixer.SetWeight(cameraB, weight);
    }
}
