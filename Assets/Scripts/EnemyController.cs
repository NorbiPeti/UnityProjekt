using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class EnemyController : CharacterControllerBase
{
    public Transform target;
    public float speed;
    public float flyForce;
    public short finalHealth = 3;
    private short _hitsToRemove;
    private readonly Random _random = new Random();
    private PlatformSpawner _platformSpawner;
    
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _platformSpawner = GameObject.FindGameObjectWithTag("Game manager").GetComponent<PlatformSpawner>();
    }

    private void OnEnable()
    {
        _hitsToRemove = finalHealth;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var tr = transform;
        if (_platformSpawner.ShouldRespawn(tr, this))
            Remove();
        var diff = target.position - tr.position;
        if (_rb.mass < 0.01f)
        { //Már lelőttük
            if (diff.magnitude > 10) //Ha már túl messze van
                Remove();

            _rb.AddForce(new Vector2(0f, flyForce * _rb.mass * _rb.gravityScale)); //Ne maradjon véletlenül útban
            return;
        }

        if (diff.y > 5 || diff.x > 20)
            Remove();

        diff.Normalize();
        float sp = ((float) _random.NextDouble() / 2f + 1f) * speed; //1 és 1.5 közötti szorzó
        _rb.AddForce(diff * (sp * _rb.mass * _rb.gravityScale));
        if (diff.x * transform.localScale.x < 0) //Ha másfelé néz, mint amerre megy
        {
            var scale = tr.localScale;
            scale.x *= -1;
            tr.localScale = scale;
        }
    }

    private void Die()
    {
        _rb.mass = 0.00001f;
        _rb.gravityScale = 0.01f;
        _rb.freezeRotation = false;
    }

    public bool IsAlive() => _rb.mass > 0.001f;

    private void HitWhileFlying()
    {
        _hitsToRemove--;
        if (_hitsToRemove == 0)
            Remove();
    }

    public void Hit()
    {
        if (IsAlive())
            Die();
        else
            HitWhileFlying();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var go = other.gameObject;
        if (!go.CompareTag("Player"))
            return;
        if (IsAlive())
            go.GetComponent<OwnCharacterController>().Hit();
    }

    private void Remove()
    {
        _rb.velocity = Vector2.zero;
        gameObject.SetActive(false);
    }
}
