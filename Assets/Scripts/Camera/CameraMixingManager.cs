using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[System.Serializable]
public struct BlendCamera
{
    public CinemachineVirtualCameraBase camera;
    public Transform originalParent;
    public float weight;
}

public class CameraMixingManager : MonoBehaviour
{
    CinemachineMixingCamera mixingCamera;

    public void AddCamera(BlendCamera blendCamera)
    {
        blendCamera.camera.transform.SetParent(transform);
        var x = mixingCamera.ChildCameras;
        mixingCamera.SetWeight(blendCamera.camera, blendCamera.weight);
        blendCamera.camera.gameObject.SetActive(true);
    }

    public void Add(BlendCamera blendCamera)
    {
        blendCamera.camera.transform.SetParent(transform);
        var x = mixingCamera.ChildCameras;
        mixingCamera.SetWeight(blendCamera.camera, blendCamera.weight);
        blendCamera.camera.gameObject.SetActive(true);
    }

    public void Add(BlendCamera[] blendCameras)
    {
        for (int i = 0; i < blendCameras.Length; i++)
        {
            blendCameras[i].camera.transform.SetParent(transform);
        }
        var x = mixingCamera.ChildCameras;
        for (int i = 0; i < blendCameras.Length; i++)
        {
            mixingCamera.SetWeight(blendCameras[i].camera, blendCameras[i].weight);
            blendCameras[i].camera.gameObject.SetActive(true);
        }
    }


    public void Remove(BlendCamera blendCamera)
    {
        blendCamera.camera.transform.SetParent(blendCamera.originalParent);
        blendCamera.camera.gameObject.SetActive(false);
    }

    public void Remove(BlendCamera[] blendCameras)
    {
        for (int i = 0; i < blendCameras.Length; i++)
        {
            BlendCamera blendCamera = blendCameras[i];

            blendCamera.camera.transform.SetParent(blendCamera.originalParent);
            blendCamera.camera.gameObject.SetActive(false);
        }
    }


    public void UpdateWeight(BlendCamera blendCamera)
    {
        mixingCamera.SetWeight(blendCamera.camera, blendCamera.weight);
    }

    public void UpdateWeight(BlendCamera[] blendCameras)
    {
        for (int i = 0; i < blendCameras.Length; i++)
        {
            mixingCamera.SetWeight(blendCameras[i].camera, blendCameras[i].weight);
        }
    }

    private void Start()
    {
        mixingCamera = GetComponent<CinemachineMixingCamera>();
    }
}
