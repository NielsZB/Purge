using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Sword : MonoBehaviour
{
    [SerializeField] FloatVariable HeatValue;
    [SerializeField] float sheathedHeatCooling = 0.2f;
    [SerializeField] float unsheathedHeatCooling = 0.01f;
    [SerializeField] float damage = 5f;
    [SerializeField] AnimationCurve heatDamageCurve = new AnimationCurve();
    [SerializeField] AnimationCurve heatGainCurve = new AnimationCurve();
    [SerializeField] float heatGain = 0.1f;
    public float Damage
    {
        get
        {
            return damage * heatDamageCurve.Evaluate(Heat);
        }
    }

    public float Heat { get; private set; }
    public bool sheathed { get; private set; } = true;
    public bool overheated { get; private set; }

    private void Update()
    {
        if(HeatValue!=null){
            HeatValue.Set(Heat);
        }
        if (Heat > 0)
        {
            if (sheathed)
            {
                Heat -= sheathedHeatCooling * Time.deltaTime;
            }
            else
            {
                if (!overheated)
                {
                    Heat -= unsheathedHeatCooling * Time.deltaTime;
                }
            }
        }


        if (overheated && Heat < 0)
        {
            overheated = false;
        }
    }

    public void GainHeat()
    {
        Heat += heatGain * heatGainCurve.Evaluate(Heat);
        if (Heat > 1)
        {
            Overcharge();
        }
    }

    public void Overcharge()
    {
        overheated = true;
    }

    public void Sheathe()
    {
        sheathed = true;
    }

    public void Unsheathe()
    {
        sheathed = false;
    }

    public void ResetHeat(){
        Heat = 0;
    }
}
