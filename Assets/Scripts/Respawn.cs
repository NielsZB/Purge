using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{

    public bool activated = false;
    private void OnTriggerEnter(Collider other)
    {
        activated = true;
    }
}
