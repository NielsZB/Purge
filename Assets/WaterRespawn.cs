using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterRespawn : MonoBehaviour
{
    RespawnCheckpoint _respawnCheckpoint;
    public GameObject startingRespawn;
    // Start is called before the first frame update
    void Start()
    {
        _respawnCheckpoint = startingRespawn.GetComponent<RespawnCheckpoint>();
    }

    void OnTriggerEnter(Collider collider)
    {
        
        if(collider.gameObject.layer == 10){
            Debug.Log("Layer correct");
            _respawnCheckpoint.Respawn();
        }
    }
}
