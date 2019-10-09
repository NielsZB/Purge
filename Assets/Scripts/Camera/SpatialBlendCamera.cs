using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SpatialBlendCamera : MonoBehaviour
{
    public CinemachineMixingCamera Mixer;
    public SpatialBlend blend;

    CinemachineBrain brain;
    [SerializeField] float smoothing;
    private void Start()
    {
        brain = Camera.main.GetComponent<CinemachineBrain>();

        if (Mixer == null)
        {
            Mixer = GetComponent<CinemachineMixingCamera>();
        }
        if (blend == null)
        {
            blend = GetComponent<SpatialBlend>();
        }
    }

    private void Update()
    {
        if (blend.HasTarget)
        {
            if (brain.ActiveVirtualCamera != (object)Mixer)
            {

            }
            for (int i = 0; i < Mixer.ChildCameras.Length; i++)
            {

                Mixer.SetWeight(i, Mathf.SmoothStep(Mixer.GetWeight(i), blend.weights[i], smoothing));
            }
        }
    }
}
