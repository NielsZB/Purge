using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerCamera : MonoBehaviour
{
    public CinemachineVirtualCamera baseCamera;
    public CinemachineVirtualCamera targetCamera;

    public void EnableTargetingCamera(bool _state)
    {
        if(_state)
        {
            baseCamera.Priority = 10;
            targetCamera.Priority = 11;
        }
        else
        {
            baseCamera.Priority = 11;
            targetCamera.Priority = 10;
        }
    }
}
