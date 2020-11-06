using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketScript : MonoBehaviour
{
    private float _fired;
    private Rigidbody2D _rb;
    private bool _goingRight;
    
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        _fired = Time.fixedTime;
        _goingRight = transform.localScale.x > 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!gameObject.activeSelf) return;
        if (Time.fixedTime - _fired > 3)
        {
            gameObject.SetActive(false);
            _rb.velocity = Vector2.zero;
        }

        _rb.AddForce(new Vector2(_goingRight ? 10 : -10, 0));
    }
}
