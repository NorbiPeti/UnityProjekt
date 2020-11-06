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

    private float _goingSince;
    private bool _goingRight;

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
        float horiz = Input.GetAxis("Horizontal");
        if (_goingSince == 0 && Mathf.Abs(horiz) > 0.1f)
        {
            _goingRight = horiz > 0;
            _goingSince = Time.time;
        }

        if (Mathf.Abs(horiz) < 0.01f)
            _goingSince = 0;

        //  Get where the camera should be, and what movement is required.
        var ownPos = _tr.position;
        Vector3 anchorPosL = ownPos + offset;
        Vector3 anchorPosR = ownPos + new Vector3(-offset.x, offset.y, offset.z);
        var targetPos = target.position;
        Vector3 movementL = targetPos - anchorPosL;
        Vector3 movementR = targetPos - anchorPosR;
        if (movementL.x > 0 && movementR.x < 0
                            && _goingSince == 0)
            return;
        var movement = (_goingSince != 0 ? _goingRight : movementL.x < movementR.x)
            ? movementL //Ha már egy ideje megyünk egy irányba vagy ha arra közelebb van a kamerának, akkor arra megy
            : movementR;

        //  Update position based on movement and speed.
        Vector3 newCamPos = ownPos + movement * speed;
        ownPos = newCamPos;
        _tr.position = ownPos;
    }
}