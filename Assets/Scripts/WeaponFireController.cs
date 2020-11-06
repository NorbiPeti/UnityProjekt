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
            var scale = rocketTransform.localScale;
            if (transform.localScale.x * scale.x < 0)
                scale.x *= -1;
            rocketTransform.localScale = scale;
            theRocket.SetActive(true);
        }
    }
}
