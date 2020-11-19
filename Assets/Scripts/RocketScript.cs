using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketScript : MonoBehaviour
{
    private float _fired;
    private Rigidbody2D _rb;
    private byte _hitCount;
    private Vector2 _forward;

    public byte maxHits = 3;
    
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        _fired = Time.fixedTime;
        _hitCount = 0;
        _forward = transform.right;
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

        //_rb.AddForce(new Vector2(_goingRight ? 10 : -10, 0));
        _rb.AddForce(10 * _forward);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (_hitCount >= maxHits)
            gameObject.SetActive(false);
        _rb.velocity = Vector2.zero;
        _hitCount++;
        if (!other.gameObject.CompareTag("Enemy"))
            return;
        var ec = other.gameObject.GetComponent<EnemyController>();
        ec.Hit();
    }
}
