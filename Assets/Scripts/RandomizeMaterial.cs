using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeMaterial : MonoBehaviour
{
    public Material jitterMaterial;
    private void Start()
    {
        Renderer render = GetComponent<Renderer>();

        Material[] jitterMaterials = new Material[render.materials.Length];
        Color[] originalColors = new Color[render.materials.Length];

        for (int i = 0; i < render.materials.Length; i++)
        {
            originalColors[i] = render.materials[i].GetColor("_BaseColor");
            jitterMaterials[i] = jitterMaterial;
        }

        render.materials = jitterMaterials;

        float RandomJitter = Random.Range(20f, 50f);

        for (int i = 0; i < jitterMaterials.Length; i++)
        {
            MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();

            render.GetPropertyBlock(propertyBlock, i);
            propertyBlock.SetColor("_BaseColor", originalColors[i]);
            propertyBlock.SetFloat("_JitterShift", RandomJitter);
            render.SetPropertyBlock(propertyBlock, i);
        }
    }
}
