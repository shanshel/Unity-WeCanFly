using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Assets.Scripts.EnumData;

[System.Serializable]

public class Sound 
{
    public SoundEnum sound;

    [Range(0f, 1f)]
    public float volume;

    [Range(.1f, 3f)]
    public float pitch;

    public bool playOnAwake = false;
    public bool isLooping = false;

    public AudioClip clip;

    public bool isMusic = false;
    [HideInInspector]
    public AudioSource audioSource;

    public bool is3D;
    [Range(2f, 100f)]
    public float maxDistanceToHear = 25f;

}
