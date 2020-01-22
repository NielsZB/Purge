using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class RandomSound : MonoBehaviour
{
    [SerializeField] AudioClip[] sounds;

    AudioSource source;

    private void Start()
    {
        source = gameObject.AddComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip)
    {
        float pitch = Random.Range(0.85f, 1.15f);

        source.pitch = pitch;
        source.clip = clip;
        source.Play();
    }

    public void PlayRandomSound()
    {
        float pitch = Random.Range(0.85f, 1.15f);

        source.pitch = pitch;
        source.clip = sounds.GetRandom();
        source.Play();
    }
}
