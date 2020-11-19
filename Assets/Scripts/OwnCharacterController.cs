using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class OwnCharacterController : CharacterControllerBase
{
    public float jumpForce;
    public float movementSpeed;
    public float sprintSpeed;
    public PlatformSpawner platformSpawner;
    
    private Vector3 _spawnPos;
    private float _health = 100f;
    private readonly Random _random = new Random();
    private readonly List<Vector3> _checkpointPosList = new List<Vector3>();
    private Vector3 _checkpointPos;
    private Animator _animator;
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int Jump = Animator.StringToHash("Jump");
    private static readonly int Sprint = Animator.StringToHash("Sprint");
    private JumpStatus _jumpStatus = JumpStatus.None;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spawnPos = transform.position;
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (platformSpawner.ShouldRespawn(transform, this))
            Respawn();
        float input = Input.GetAxis("Horizontal");
        var tr = transform;
        if (input < 0 && tr.localScale.x > 0
            || input > 0 && tr.localScale.x < 0)
        {
            var scale = tr.localScale;
            scale.x *= -1;
            tr.localScale = scale;
        }

        bool sprinting = Input.GetButton("Fire3");
        if (sprinting)
            input *= sprintSpeed;

        if (Mathf.Abs(_rb.velocity.x) <= 3)
            _rb.velocity += new Vector2(input * movementSpeed, 0);

        if (Input.GetButtonDown("Jump") && IsOnGround())
        {
            //_rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            _rb.velocity += new Vector2(0, jumpForce);
            if (sprinting) //Csak akkor animálja az ugrást, ha fut közben
            {
                _jumpStatus = JumpStatus.Up;
                _animator.SetInteger(Jump, (int) _jumpStatus);
            }
        }
        else if (_jumpStatus == JumpStatus.Up && _rb.velocity.y <= 0)
        {
            _jumpStatus = JumpStatus.Down;
            _animator.SetInteger(Jump, (int) _jumpStatus);
        }
        else if (_jumpStatus == JumpStatus.Down && Math.Abs(_rb.velocity.y) <= 0.001f)
        {
            _jumpStatus = JumpStatus.None;
            _animator.SetInteger(Jump, (int) _jumpStatus);
        }

        if (_checkpointPos.x > 0 && (tr.position - _checkpointPos).magnitude < 2f)
        {
            _spawnPos = _checkpointPos;
            _checkpointPosList.RemoveAt(0);
            _checkpointPos = _checkpointPosList.Count > 0 ? _checkpointPosList[0] : Vector3.zero;
        }

        _animator.SetFloat(Speed, Math.Abs(_rb.velocity.x));
        _animator.SetBool(Sprint, Input.GetButton("Fire3"));
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

    public void SetCheckpoint(Vector3 pos)
    {
        _checkpointPosList.Add(pos);
        if (_checkpointPos.x <= 0) _checkpointPos = _checkpointPosList[0];
    }

    enum JumpStatus
    {
        None,
        Up,
        Down
    }
}