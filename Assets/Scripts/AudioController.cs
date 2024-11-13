using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public Audio[] audios;

    public AudioClip GetAudio(string audioName)
    {
        if (audios == null || audios.Length == 0)
        {
            return null;
        }
        AudioClip res = audios.FirstOrDefault(a => a.name == audioName)?.audio;
        return res;
    }
}