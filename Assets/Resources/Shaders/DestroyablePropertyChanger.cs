using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyablePropertyChanger : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject [] Destroyables;
    void Start()
    {
        foreach(GameObject go in Destroyables){
            go.GetComponent<MeshRenderer>().material.SetFloat("Vector1_1D9D62C",Random.Range(1,100));
        }
    }
}
