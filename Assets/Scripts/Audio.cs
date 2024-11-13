using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Audio
{
    public string name;
    public AudioClip audio;
    [Range(0f, 1f)] public float volume;
    [Range(0.1f, 1f)] public float pitch;
}
