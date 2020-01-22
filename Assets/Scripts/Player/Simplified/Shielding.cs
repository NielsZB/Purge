using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shielding : MonoBehaviour
{
    [SerializeField] FloatVariable StaminaVariable;
    [SerializeField] AnimationCurve wardWeightCurve;
    [SerializeField] AnimationCurve shieldSizeXCurve;
    [SerializeField] AnimationCurve shieldSizeYCurve;
    [SerializeField] AnimationCurve shieldSizeZCurve;
    [Space(10f)]
    [SerializeField, GradientUsage(true)] Gradient emmisionColorGradient;
    [SerializeField] AnimationCurve fresnelAlpha;
    [SerializeField] AnimationCurve emmisionFresnelPower;

    [SerializeField] float wardDuration;
    [SerializeField] Transform ward;
    [SerializeField] float cost;
    [SerializeField] float rechargeSpeed;
    [SerializeField] float rechargeFreezeDuration;
    PlayerHealth healthModule;
    WardScriptTest pushback;
    Animator animatorModule;
    public float Stamina { get; private set; } = 1;
    float timeBeforeCharge = 0;
    bool canRecharge;
    bool isShielding;

    Renderer render;

    private void Start()
    {
        healthModule = GetComponent<PlayerHealth>();
        pushback = GetComponent<WardScriptTest>();
        animatorModule = GetComponent<Animator>();
        render = ward.gameObject.GetComponent<Renderer>();

        if (StaminaVariable != null)
        {
            StaminaVariable.Set(Stamina);
        }
    }

    private void Update()
    {
        if (canRecharge)
        {
            if (Stamina < 1)
            {
                Stamina += rechargeSpeed * Time.deltaTime;

                if (StaminaVariable != null)
                {
                    StaminaVariable.Set(Stamina);
                }
            }
        }

        if (timeBeforeCharge > 0)
        {
            timeBeforeCharge -= Time.deltaTime;
        }
        else
        {
            canRecharge = true;
        }
    }

    public void Activate()
    {
        if (!isShielding)
        {

            if (Stamina - cost >= 0)
            {
                Stamina -= cost;
                if (StaminaVariable != null)
                {
                    StaminaVariable.Set(Stamina);
                }

                StartCoroutine(Warding());
                pushback.Push();
                canRecharge = false;
                timeBeforeCharge = rechargeFreezeDuration;
            }
        }
    }

    IEnumerator Warding()
    {
        isShielding = true;
        ward.gameObject.SetActive(true);
        healthModule.EnableInvulnerability();
        Material material = render.sharedMaterial;
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / wardDuration;

            material.SetColor("_EmissiveColor", emmisionColorGradient.Evaluate(t));
            material.SetFloat("_EmissionFresnelPower", emmisionFresnelPower.Evaluate(t));
            material.SetFloat("_FresnelAlpha", fresnelAlpha.Evaluate(t));

            ward.localScale = new Vector3(shieldSizeXCurve.Evaluate(t), shieldSizeYCurve.Evaluate(t), shieldSizeZCurve.Evaluate(t));
            animatorModule.SetLayerWeight(1, wardWeightCurve.Evaluate(t));
            yield return null;
        }
        healthModule.DisableInvulnerability();
        isShielding = false;
        ward.gameObject.SetActive(false);
    }



    public void ResetStamina()
    {
        Stamina = 1;
    }
}
