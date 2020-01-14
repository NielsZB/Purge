using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shielding : MonoBehaviour
{
    [SerializeField] AnimationCurve wardWeightCurve;
    [SerializeField] AnimationCurve shieldSizeXCurve;
    [SerializeField] AnimationCurve shieldSizeYCurve;
    [SerializeField] AnimationCurve shieldSizeZCurve;
    [SerializeField] float wardDuration;
    [SerializeField] Transform ward;
    PlayerHealth healthModule;
    WardScriptTest pushback;
    Animator animatorModule;
    private void Start()
    {
        healthModule = GetComponent<PlayerHealth>();
        pushback = GetComponent<WardScriptTest>();
        animatorModule = GetComponent<Animator>();
    }
    public void Activate()
    {
        StartCoroutine(Warding());
        pushback.BlastOff();
    }

    IEnumerator Warding()
    {
        ward.gameObject.SetActive(true);
        healthModule.EnableInvulnerability();
        float t = 0;
        while(t < 1)
        {
            t += Time.deltaTime / wardDuration;
            ward.localScale = new Vector3(shieldSizeXCurve.Evaluate(t), shieldSizeYCurve.Evaluate(t), shieldSizeZCurve.Evaluate(t));
            animatorModule.SetLayerWeight(1, wardWeightCurve.Evaluate(t));
            yield return null;
        }
        healthModule.DisableInvulnerability();
        ward.gameObject.SetActive(false);
    }
}
