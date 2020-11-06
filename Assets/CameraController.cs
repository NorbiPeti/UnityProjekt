using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Class for following the player with the camera
public class CameraController : MonoBehaviour {
    //  Camera speed
    public float speed = 0.1f;
    //  GameObject to be followed.
    public Transform target;
    //  Our own transform
    private Transform _tr;

    //  Center position of the target relative to the camera.
    public Vector3 offset;
    private float _lastSwitch = -1;
    private bool _facingRight;

    //  Store initial values
    void Start()
    {
        _tr = transform;
        offset = target.position - _tr.position;
    }

    //  Update positions
    void FixedUpdate()
    {
        if (!target) return;
        //  Get where the camera should be, and what movement is required.
        var position = _tr.position;
        bool facingLeft = target.localScale.x < 0;
        Vector3 anchorPos = position + (facingLeft ? new Vector3(-offset.x, offset.y, offset.z) : offset);
        Vector3 movement = target.position - anchorPos;

        float sp = speed;
        /*if (_lastSwitch >= 0 && Time.time - _lastSwitch < 0.5f)
            sp /= 4;*/
        float diff = Time.time - _lastSwitch;
        if (_lastSwitch >= 0 && diff < 2f)
            sp *= 0.5f + diff / 4;

        if (facingLeft == _facingRight) //Megváltozott az irány
            _lastSwitch = Time.time;

        _facingRight = !facingLeft;
        
        //  Update position based on movement and speed.
        Vector3 newCamPos = position + movement * sp;
        position = newCamPos;
        _tr.position = position;
    }
}