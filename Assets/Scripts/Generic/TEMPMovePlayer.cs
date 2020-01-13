using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TEMPMovePlayer : MonoBehaviour
{
    public CinemachineDollyCart cart;
    public float speed;
    void Update()
    {
        cart.m_Position += Input.GetAxis("LeftHorizontal") * speed;
    }
}
