using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering;

public class CameraMixer : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    CinemachineVirtualCamera currentVirtualCamera;
    Volume volume;
    CinemachineBrain brain;
    private void Start()
    {
        brain = Camera.main.GetComponent<CinemachineBrain>();
        virtualCamera.enabled = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        virtualCamera.enabled = true;
        currentVirtualCamera = (CinemachineVirtualCamera)brain.ActiveVirtualCamera;
        currentVirtualCamera.m_Priority = 5;
        virtualCamera.m_Priority = 10;
    }
}
