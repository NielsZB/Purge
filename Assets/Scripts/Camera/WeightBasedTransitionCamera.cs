using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class WeightBasedTransitionCamera : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCameraBase cameraA = null;
    [SerializeField] CinemachineVirtualCameraBase cameraB = null;

    WeightVolume volume;
    CinemachineMixingCamera mixer;

    Transform cameraADefaultParent;
    Transform cameraBDefaultParent;
    private void Start()
    {
        volume = GetComponent<WeightVolume>();

        if (mixer == null)
        {
            mixer = (CinemachineMixingCamera)gameObject.AddComponent(typeof(CinemachineMixingCamera));
        }
        mixer.Priority = 99;
        mixer.enabled = false;

        cameraADefaultParent = cameraA.transform.parent;
        cameraBDefaultParent = cameraB.transform.parent;
    }


    private void FixedUpdate()
    {
        if (volume.HasTarget)
        {
            if (cameraA.transform.parent == transform && cameraB.transform.parent == transform)
            {
                mixer.SetWeight(cameraA, 1f - volume.Weight);
                mixer.SetWeight(cameraB, volume.Weight);
            }
        }
        else
        {
            if (mixer.enabled)
            {
                float weight = volume.Weight;

                if (weight == 0f)
                {
                    cameraA.transform.SetParent(cameraADefaultParent, true);
                    cameraB.transform.SetParent(cameraBDefaultParent, true);

                    cameraA.Priority = 11;
                    cameraB.Priority = 10;

                    var x = mixer.ChildCameras;

                    mixer.enabled = false;
                }
                else if (weight == 1f)
                {
                    cameraA.transform.SetParent(cameraADefaultParent, true);
                    cameraB.transform.SetParent(cameraBDefaultParent, true);

                    cameraA.Priority = 10;
                    cameraB.Priority = 11;

                    var x = mixer.ChildCameras;

                    mixer.enabled = false;
                }


            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (mixer == null)
        {
            mixer = (CinemachineMixingCamera)gameObject.AddComponent(typeof(CinemachineMixingCamera));
        }
        mixer.enabled = true;

        float weight = volume.GetWeight();

        cameraA.transform.SetParent(transform, true);
        cameraA.enabled = true;

        cameraB.transform.SetParent(transform, true);
        cameraB.enabled = true;


        var x = mixer.ChildCameras;

        mixer.SetWeight(cameraA, 1f - weight);
        mixer.SetWeight(cameraB, weight);
    }
}
