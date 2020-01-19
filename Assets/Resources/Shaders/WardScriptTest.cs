using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WardScriptTest : MonoBehaviour
{
    GameObject[] DestroyablesArray;
    public float wardRange = 3f;
    float currentDistance;
    public float wardForce = 500f;
    public float wardUpForce = 200f;
    public float torqueForce = 50f;
    public float angularDragOverTime;
    public float maxDrag;
    public float dragIncreaseSpeed;

    Rigidbody currentRB;

    [SerializeField] LayerMask mask;

    //void Start()
    //{
    //    DestroyablesArray = GameObject.FindGameObjectsWithTag("Destroyable");

    //    foreach (GameObject go in DestroyablesArray)
    //    {
    //        Rigidbody body = go.AddComponent<Rigidbody>();
    //        body.isKinematic = true;
    //        body.mass = .5f;
    //        go.AddComponent<MeshCollider>();
    //        go.GetComponent<MeshCollider>().convex = true;
    //    }
    //}

    public void Push()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position + Vector3.up, wardRange, mask);

        for (int i = 0; i < colliders.Length; i++)
        {
            Collider collider = colliders[i];

            if (collider.TryGetComponent(out EnemyHealth enemy))
            {
                enemy.Stun();
            }
            else
            {
                if (!collider.TryGetComponent(out Rigidbody rigidbody))
                {
                    rigidbody = collider.gameObject.AddComponent<Rigidbody>();
                }

                rigidbody.mass = .5f;
                rigidbody.isKinematic = false;

                float distance = (rigidbody.position - transform.position).magnitude;
                Vector3 direction = (rigidbody.position - transform.position).normalized;

                rigidbody.AddForce(direction * distance.Remap01(wardRange, 0) * wardForce + (Vector3.up * wardUpForce));
                rigidbody.AddTorque(Random.insideUnitSphere * wardForce, ForceMode.Impulse);
                StartCoroutine(FreezeTimer(rigidbody));
            }
        }
    }

    //public void BlastOff()
    //{
    //    foreach (GameObject go in DestroyablesArray)
    //    {
    //        currentDistance = Vector3.Distance(transform.position, go.transform.position);
    //        if (currentDistance < wardRange)
    //        {
    //            currentRB = go.GetComponent<Rigidbody>();
    //            currentRB.isKinematic = false;
    //            currentRB.AddForce((go.transform.position - transform.position) * wardForce + transform.up * wardUpForce);
    //            currentRB.AddTorque(Random.insideUnitSphere * wardForce, ForceMode.Impulse);
    //            //go.GetComponent<MeshRenderer>().material.SetFloat("Vector1_33A50FC4", 0f);
    //            StartCoroutine(FreezeTimer(currentRB));
    //        }
    //    }
    //}

    IEnumerator FreezeTimer(Rigidbody rigidbody)
    {
        yield return new WaitForSeconds(.2f);

        rigidbody.useGravity = false;
        float current_drag = 0;
        float current_angularDrag = 0;

        while (current_drag <= maxDrag)
        {
            current_drag = current_drag + dragIncreaseSpeed;
            current_angularDrag += angularDragOverTime;

            rigidbody.drag = current_drag;
            rigidbody.angularDrag = current_angularDrag;

            yield return null;
        }
        rigidbody.isKinematic = true;
        rigidbody.drag = 0;
        rigidbody.angularDrag = 0;
    }

}
