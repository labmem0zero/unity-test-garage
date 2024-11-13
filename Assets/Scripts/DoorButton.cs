using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorButton : MonoBehaviour
{
    public AnimationController animationController;

    private bool _clicked;

    private float _btnDown = 2.875f;
    private float _btnUp = 2.890f;

    public void Press()
    {
        if (!_clicked)
        {
            animationController.OpenDoor();
            _clicked=true;
            transform.position.Set(_btnDown, transform.position.y, transform.position.z);
            return;
        }
        animationController.CloseDoor();
        _clicked = false;
        transform.position.Set(_btnUp, transform.position.y, transform.position.z);
    }
}
