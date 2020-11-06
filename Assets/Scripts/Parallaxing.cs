using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxing : MonoBehaviour
{
   	private Transform _background;
    private float _parallaxScale;
	public float smoothing = 1f;

	private Transform _cam;				// reference to the main cameras transform
	private Vector3 _previousCamPos;	// the position of the camera in the previous frame

	// Is called before Start(). Great for references.
	void Awake () {
		// set up the camera reference
		_cam = Camera.main.transform;
		_background = transform;
	}

    // Start is called before the first frame update
    void Start()
    {
        // The previous frame had the current frame's camera position
		_previousCamPos = _cam.position;

        // asigning coresponding parallaxScale
        _parallaxScale = _background.position.z * -1;
    }

    // Update is called once per frame
    void Update()
    {
	    // the parallax is the opposite of the camera movement because the previous frame multiplied by the scale
	    float parallax = (_previousCamPos.x - _cam.position.x) * _parallaxScale;

	    // set a target x position which is the current position plus the parallax
	    var position = _background.position;
	    float backgroundTargetPosX = position.x + parallax;

	    // create a target position which is the background's current position with it's target x position
	    Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, position.y, position.z);

	    // fade between current position and the target position using lerp
	    position = Vector3.Lerp(position, backgroundTargetPos, smoothing * Time.deltaTime);
	    _background.position = position;

	    // set the previousCamPos to the camera's position at the end of the frame
	    _previousCamPos = _cam.position;
    }
}