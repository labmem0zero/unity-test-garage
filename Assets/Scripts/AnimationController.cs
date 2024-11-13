using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public Animator anim;
    public AudioSource doorAudio;
    public AudioController audioController;

    void Awake()
    {
        anim.SetBool("DoorOpen", false);
        anim.StopPlayback();
    }

    public void OpenDoor()
    {
        doorAudio.clip = audioController.GetAudio("door_open");
        doorAudio.Play();
        anim.SetBool("DoorOpen", true);
    }

    public void CloseDoor()
    {
        doorAudio.clip = audioController.GetAudio("door_close");
        doorAudio.Play();
        anim.SetBool("DoorOpen", false);
    }
}
