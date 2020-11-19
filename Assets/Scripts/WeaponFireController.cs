using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponFireController : MonoBehaviour
{
    public GameObject prefab;
    public Transform firePoint;
    private ObjectPool _pool;
    
    // Start is called before the first frame update
    void Start()
    {
        _pool = new ObjectPool(prefab, 10);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            var theRocket = _pool.GetObject();
            var rocketTransform = theRocket.transform;
            rocketTransform.position = firePoint.position;
            /*var scale = rocketTransform.localScale;
            if (transform.localScale.x * scale.x < 0)
                scale.x *= -1;
            rocketTransform.localScale = scale;*/
             
            // vector from this object towards the target location
            var vectorToTarget = Camera.main.ScreenToWorldPoint(Input.mousePosition) - rocketTransform.position;
            // rotate that vector by 90 degrees around the Z axis
            Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, 90) * vectorToTarget;
 
            // get the rotation that points the Z axis forward, and the Y axis 90 degrees away from the target
            // (resulting in the X axis facing the target)
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, rotatedVectorToTarget);
            
            rocketTransform.rotation = targetRotation;
            Debug.Log("Rotation: " + rocketTransform.rotation.eulerAngles);
            /*Debug.Log("Rocket position: " + rocketTransform.position);
            Debug.Log("Target position: " + Camera.main.ScreenToWorldPoint(Input.mousePosition));
            Debug.Log("Difference: " + direction);*/
            theRocket.SetActive(true);
        }
    }
}
