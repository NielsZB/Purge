using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyablePropertyChanger : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject [] Destroyables;
    void Start()
    {
        Destroyables = GameObject.FindGameObjectsWithTag("Destroyable");
        foreach(GameObject go in Destroyables){
            go.GetComponent<MeshRenderer>().material.SetFloat("Vector1_1D9D62C",Random.Range(1,100));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
