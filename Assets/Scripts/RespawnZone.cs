using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnZone : MonoBehaviour
{
    [SerializeField] FloatVariable fade;
    [SerializeField] float duration;
    [SerializeField] AnimationCurve curve;
    [SerializeField] Respawn spawn;

    Transform player;
    private void OnTriggerEnter(Collider other)
    {
        if (spawn.activated)
        {
            player = other.transform;
        }
    }

    IEnumerator FadeInOut()
    {
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime / duration;

            fade.Set(curve.Evaluate(t));

            yield return null;
        }

    }
}
