using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{

    public string name;
    public AudioClip clip;
    public bool loop = false;

    [Range(0,1)]
    public float volume = 1;

    [HideInInspector]
    public AudioSource source;

}
