using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OwnCharacterController : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Vector3 _spawnPos;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spawnPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(_rb.velocity.x) > 3)
            return;
        float input = Input.GetAxis("Horizontal");
        if (input < 0 && _rb.transform.localScale.x > 0
            || input > 0 && _rb.transform.localScale.x < 0)
        {
            var tr = transform;
            var scale = tr.localScale;
            scale.x *= -1;
            tr.localScale = scale;
        }

        if (Input.GetButton("Fire3"))
            input *= 10;
        _rb.AddForce(new Vector2(input * 5, 0));

        if (Input.GetButtonDown("Jump") && IsOnGround())
            _rb.AddForce(new Vector2(0, 4), ForceMode2D.Impulse);
    }

    public void Hit()
    {
        Respawn();
    }

    private void Respawn()
    {
        transform.position = _spawnPos;
    }

    private bool IsOnGround()
    {
        var res = new List<Collider2D>();
        _rb.OverlapCollider(new ContactFilter2D(), res);
        return res.Any(col => col.CompareTag("Ground"));
    }
}