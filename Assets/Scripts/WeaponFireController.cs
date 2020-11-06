using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponFireController : MonoBehaviour
{
    public Rigidbody2D prefab;
    public Transform firePoint;
    private List<Rigidbody2D> rockets = new List<Rigidbody2D>(10);
    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 10; i++)
            rockets.Add(Instantiate(prefab));
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            Rigidbody2D theRocket = null;
            foreach (var rocket in rockets)
            {
                if (!rocket.gameObject.activeSelf)
                {
                    theRocket = rocket;
                    break;
                }
            }

            if (theRocket is null)
                rockets.Add(theRocket = Instantiate(prefab));
            var rocketTransform = theRocket.transform;
            rocketTransform.position = firePoint.position;
            var scale = rocketTransform.localScale;
            if (transform.localScale.x * scale.x < 0)
                scale.x *= -1;
            rocketTransform.localScale = scale;
            theRocket.gameObject.SetActive(true);
        }
    }
}
