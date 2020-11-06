using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Class for following the player with the camera
public class CameraController : MonoBehaviour {
    //  Camera speed
    public float speed = 0.05f;
    //  GameObject to be followed.
    public Transform target;
    //  Our own transform
    private Transform tr;

    //  Center position of the target relative to the camera.
    public Vector3 offset;

    //  Store initial values
    void Start()
    {
        tr = transform;
        offset = target.position - tr.position;
    }

    //  Update positions
    void FixedUpdate ()
    {
        if (!target) return;
        //  Get where the camera should be, and what movement is required.
        var position = tr.position;
        Vector3 anchorPos = position + offset;
        Vector3 movement = target.position - anchorPos;

        //  Update position based on movement and speed.
        Vector3 newCamPos = position + movement*speed;
        position = newCamPos;
        tr.position = position;
    }
}