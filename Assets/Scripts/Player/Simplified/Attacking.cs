using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacking : MonoBehaviour
{
    [SerializeField] float sheathedHeatCooling;
    [SerializeField] float unsheathedHeatCooling;
    [SerializeField] float heatGain;
    public float damage;
    public float Heat { get; private set; }
    public bool sheathed { get; private set; }
    public bool overheated { get; private set; }

    private void Update()
    {
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
        Heat += heatGain;
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
}
