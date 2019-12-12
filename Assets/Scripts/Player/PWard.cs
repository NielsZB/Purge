using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PWard : MonoBehaviour
{
    [SerializeField] GameObject shield;
    
    [SerializeField] float wardDuration = 0.25f;
    [SerializeField] float wardCooldown = 1f;
    [Space(10f)]
    [SerializeField] float burstDuration = 0.25f;
    [SerializeField] float burstCooldown = 1f;
    public bool IsActive { get; private set; }
    public bool OnCooldown { get; private set; }

    float duration;
    float cooldown;

    WaitForEndOfFrame endOfFrame;

    Health health;
    private void Start()
    {
        endOfFrame = new WaitForEndOfFrame();
        health = GetComponent<Health>();
    }

    public void Ward()
    {
        if (IsActive || OnCooldown)
            return;

        duration = wardDuration;
        cooldown = wardCooldown;
        StartCoroutine(AbilityActive());
    }
    public void Burst()
    {
        if (IsActive || OnCooldown)
            return;

        duration = burstDuration;
        cooldown = burstCooldown;
        StartCoroutine(AbilityActive());
    }

    IEnumerator AbilityActive()
    {
        IsActive = true;
        shield.SetActive(true);
        float t = 0;
        health.SetVulnerability(false);

        while (t < 1)
        {
            t += Time.deltaTime / duration;
            yield return endOfFrame;
        }

        health.SetVulnerability(true);
        shield.SetActive(false);
        StartCoroutine(Cooldown());
        IsActive = false;
    }

    IEnumerator Cooldown()
    {
        OnCooldown = true;
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime / duration;
            yield return endOfFrame;
        }

        OnCooldown = false;
    }
}
