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
    private List<Vector3> _checkpointPosList = new List<Vector3>();
    private Vector3 _checkpointPos;

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
        var tr = transform;
        if (input < 0 && tr.localScale.x > 0
            || input > 0 && tr.localScale.x < 0)
        {
            var scale = tr.localScale;
            scale.x *= -1;
            tr.localScale = scale;
        }

        if (Input.GetButton("Fire3"))
            input *= sprintSpeed;
        _rb.AddForce(new Vector2(input * movementSpeed, 0));

        if (Input.GetButtonDown("Jump") && IsOnGround())
            _rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);

        if (_checkpointPos.x > 0 && (tr.position - _checkpointPos).magnitude < 2f)
        {
            _spawnPos = _checkpointPos;
            _checkpointPosList.RemoveAt(0);
            _checkpointPos = _checkpointPosList.Count > 0 ? _checkpointPosList[0] : Vector3.zero;
        }
    }

    public void Hit()
    {
        _health -= (float) _random.NextDouble() % 20f + 20f;
        if (_health <= 0f)
            Respawn();
    }

    public void Respawn()
    {
        transform.position = _spawnPos;
        _health = 100f;
        _rb.velocity = Vector2.zero;
    }

    public bool IsOnGround(string groundName = "")
    {
        var res = new List<Collider2D>();
        _rb.OverlapCollider(new ContactFilter2D(), res);
        return res.Any(col => col.CompareTag("Ground") && (groundName.Length == 0 || col.name.StartsWith(groundName)));
    }

    public void SetCheckpoint(Vector3 pos)
    {
        _checkpointPosList.Add(pos);
        if (_checkpointPos.x <= 0) _checkpointPos = _checkpointPosList[0];
    }
}