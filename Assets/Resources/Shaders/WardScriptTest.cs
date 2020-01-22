using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WardScriptTest : MonoBehaviour
{
    public float wardRange = 3f;
    public float wardForce = 500f;
    public float wardUpForce = 200f;
    public float torqueForce = 50f;
    public float angularDragOverTime;
    public float maxDrag;
    public float dragIncreaseSpeed;


    [SerializeField] LayerMask mask;
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

                Vector3 direction = (rigidbody.position - transform.position).normalized;

                rigidbody.AddForce(direction * wardForce + (Vector3.up * wardUpForce));
                rigidbody.AddTorque(Random.insideUnitSphere * wardForce, ForceMode.Impulse);
                StartCoroutine(FreezeTimer(rigidbody));
            }
        }
    }

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
