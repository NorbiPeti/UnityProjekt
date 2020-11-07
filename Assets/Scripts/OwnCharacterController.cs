using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class OwnCharacterController : MonoBehaviour
{
    public float jumpForce;
    public float movementSpeed;
    public float sprintSpeed;
    
    private Rigidbody2D _rb;
    private Vector3 _spawnPos;
    private float _health = 100f;
    private Random _random = new Random();

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
            input *= sprintSpeed;
        _rb.AddForce(new Vector2(input * movementSpeed, 0));

        if (Input.GetButtonDown("Jump") && IsOnGround())
            _rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
    }

    public void Hit()
    {
        _health -= (float) _random.NextDouble() % 20f + 20f;
        if (_health <= 0f)
            Respawn();
    }

    private void Respawn()
    {
        transform.position = _spawnPos;
        _health = 100f;
    }

    private bool IsOnGround()
    {
        var res = new List<Collider2D>();
        _rb.OverlapCollider(new ContactFilter2D(), res);
        return res.Any(col => col.CompareTag("Ground"));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var tr = other.transform;
        if(!tr.CompareTag("Checkpoint"))
            return;
        _spawnPos = tr.position;
    }
}