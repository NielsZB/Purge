using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WardScriptTest : MonoBehaviour
{
    GameObject [] DestroyablesArray;
    public float wardRange = 3f;
    float currentDistance;
    public float wardForce = 500f;
    public float wardUpForce = 200f;
    public float torqueForce = 50f;
    public float angularDragOverTime;
    public float maxDrag;
    public float dragIncreaseSpeed;

    Rigidbody currentRB;
    

    void Start()
    {
        DestroyablesArray = GameObject.FindGameObjectsWithTag("Destroyable");
        Debug.Log(DestroyablesArray.Length);
        foreach(GameObject go in DestroyablesArray){
            Rigidbody body = go.AddComponent<Rigidbody>();
            body.isKinematic = true;
            body.mass = .5f;
            go.AddComponent<MeshCollider>();
            go.GetComponent<MeshCollider>().convex = true;
        }
    }

    public void BlastOff()
    {
        foreach (GameObject go in DestroyablesArray)
        {
            currentDistance = Vector3.Distance(transform.position, go.transform.position);
            if (currentDistance < wardRange)
            {
                currentRB = go.GetComponent<Rigidbody>();
                currentRB.isKinematic = false;
                currentRB.AddForce((go.transform.position - transform.position) * wardForce + transform.up * wardUpForce);
                currentRB.AddTorque(Random.insideUnitSphere * wardForce, ForceMode.Impulse);
                //go.GetComponent<MeshRenderer>().material.SetFloat("Vector1_33A50FC4", 0f);
                StartCoroutine(freezeTimer(go));
            }
        }
    }

    IEnumerator freezeTimer(GameObject go){
        yield return new WaitForSeconds(.2f);
        Rigidbody slowRB = go.GetComponent<Rigidbody>();
        float current_drag = 0;
        float current_angularDrag = 0;
         while(current_drag <= maxDrag){
             current_drag = current_drag + dragIncreaseSpeed;
            current_angularDrag += angularDragOverTime;
             slowRB.drag = current_drag;
             slowRB.angularDrag = current_angularDrag;
             /*if(current_drag >=40){
                 go.GetComponent<MeshRenderer>().material.SetFloat("Vector1_33A50FC4", 0.25f);
             }*/
             yield return null;
         }
         go.GetComponent<Rigidbody>().isKinematic = true;
         slowRB.drag = 0;
     }

}
