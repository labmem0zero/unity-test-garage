using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Input : MonoBehaviour
{
    public TextMeshProUGUI text;
    public CharacterController controller;
    private float _sensitivity = 40;
    private float _speed = 3f;
    public float _vSpeed = 0;
    private float _useDistance = 2f;
    private float _throwForce = 150f;
    private Camera _cam;
    private Vector2 _rotation = Vector2.zero;
    private Transform _highlight;
    private string _highlightName;
    private Transform _player;
    private Transform _inHands;
    private Transform _world;

    void Start()
    {
        _highlight = transform;
        _cam = this.transform.GetComponentInChildren<Camera>();
        if (_cam == null)
        {
            print("Камера игрока не найдена");
            Application.Quit();
        }

        _player = this.transform;
        _world = _player.parent;
        text.rectTransform.anchoredPosition = new Vector2(0, 0-Screen.height/2+50);
    }

    void FixedUpdate()
    {
        if (UnityEngine.Input.GetKey("w"))
        {
            controller.Move(_speed * Time.fixedDeltaTime * transform.forward);
        }
        if (UnityEngine.Input.GetKey("a"))
        {
            controller.Move(_speed * Time.fixedDeltaTime * transform.right*-1);
        }
        if (UnityEngine.Input.GetKey("s"))
        {
            controller.Move(_speed * Time.fixedDeltaTime * transform.forward*-1);
        }
        if (UnityEngine.Input.GetKey("d"))
        {
            controller.Move(_speed * Time.fixedDeltaTime * transform.right);
        }

        _vSpeed = !controller.isGrounded ? _vSpeed - Time.fixedDeltaTime * 9.8f : 0;
        controller.Move(_vSpeed * Vector3.up*Time.fixedDeltaTime);
        _rotation.x += UnityEngine.Input.GetAxis("Mouse X") * _sensitivity;
        _rotation.y += UnityEngine.Input.GetAxis("Mouse Y") * _sensitivity;
        _rotation.y = Mathf.Clamp(_rotation.y, -75f, 75f);
        var xQuat = Quaternion.AngleAxis(_rotation.x, Vector3.up);
        var yQuat = Quaternion.AngleAxis(_rotation.y, Vector3.left);
        _cam.transform.localRotation = yQuat;
        transform.rotation = xQuat;
    }

    private void HighlightON(Transform obj)
    {
        _highlight = obj;
        _highlightName = obj.name;
        var rnd = _highlight.GetComponentInChildren<Renderer>();
        rnd.material.color = new Color(0.5f, 1, 0.3f);
    }

    private void HighlightOFF()
    {
        _highlight.GetComponentInChildren<Renderer>().material.color = new Color(1, 1, 1);
        _highlight = null;
        _highlightName = "";
    }
    private void Update()
    {
        Ray ray = _cam.ScreenPointToRay(new Vector3(Screen.width/2, Screen.height/2, 0));
        RaycastHit hit;
        bool takenItem = false;
        if (Physics.Raycast(ray, out hit))
        {
            var obj = hit.transform;
            if (!String.IsNullOrEmpty(_highlightName) && (_highlightName != obj.name || hit.distance>_useDistance))
            {
                HighlightOFF();
            }

            if (obj.CompareTag("Usable") && hit.distance <= _useDistance)
            {
                HighlightON(obj);
                if (UnityEngine.Input.GetKeyDown("e"))
                {
                   Use(obj);   
                }
            }

            if (obj.CompareTag("Item") && hit.distance<=_useDistance && !_inHands)
            {
                HighlightON(obj);
                if (UnityEngine.Input.GetKeyDown("e"))
                {
                    takenItem=Use(obj);   
                }
            }
        }
        if (!takenItem && (UnityEngine.Input.GetMouseButtonDown(0)||UnityEngine.Input.GetKeyDown("e")))
        {
            DropItem();
        }
    }

    private bool Use(Transform obj)
    {
        if (obj.name=="button"){
            obj.GetComponentInChildren<DoorButton>().Press();
            return false;
        }
        TakeItem(obj);
        return true;
    }

    private void TakeItem(Transform obj)
    {
        if (_inHands|| !obj.CompareTag("Item"))
        {
            print("Hands busy or no tag");
            return;
        }
        obj.SetParent(_cam.transform);
        var rigid = obj.GetComponentInChildren<Rigidbody>();
        rigid.useGravity = false;
        rigid.isKinematic = true;
        obj.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        _inHands = obj;
    }

    private void DropItem()
    {
        if (_inHands == null)
        {
            return;
        }

        var rigid = _inHands.GetComponentInChildren<Rigidbody>(); 
        rigid.useGravity = true;
        rigid.isKinematic = false;
        _inHands.SetParent(_world);
        _inHands.gameObject.layer = LayerMask.NameToLayer("Default");
        rigid.AddForce(_cam.transform.forward*_throwForce);
        _inHands = null;
    }
}
