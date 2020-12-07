using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponFireController : MonoBehaviour
{
    public GameObject prefab;
    public Transform firePoint;
    private ObjectPool _pool;
    private ParticleSystem _particle;
    
    // Start is called before the first frame update
    void Start()
    {
        _pool = new ObjectPool(prefab, 10);
        _particle = firePoint.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            var theRocket = _pool.GetObject();
            var rocketTransform = theRocket.transform;
            rocketTransform.position = firePoint.position;
             
            //https://forum.unity.com/threads/look-rotation-2d-equivalent.611044/
            // vector from this object towards the target location
            var vectorToTarget = Camera.main.ScreenToWorldPoint(Input.mousePosition) - rocketTransform.position;
            // rotate that vector by 90 degrees around the Z axis
            Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, 90) * vectorToTarget;
 
            // get the rotation that points the Z axis forward, and the Y axis 90 degrees away from the target
            // (resulting in the X axis facing the target)
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, rotatedVectorToTarget);
            
            rocketTransform.rotation = targetRotation;
            theRocket.SetActive(true);
        }

        if (Input.GetButtonDown("Fire2"))
            _particle.Play();
        else if (Input.GetButtonUp("Fire2"))
            _particle.Stop();
    }
}
