using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform target;
    public float speed;
    public float flyForce;
    private Rigidbody2D _rb;
    
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var tr = transform;
        var diff = target.position - tr.position;
        if (_rb.mass < 0.01f)
        { //Már lelőttük
            if (diff.magnitude > 10)
            { //Ha már túl messze van
                _rb.velocity = Vector2.zero;
                gameObject.SetActive(false);
            }

            _rb.AddForce(new Vector2(0f, flyForce * _rb.mass * _rb.gravityScale)); //Ne maradjon véletlenül útban
            return;
        }

        diff.Normalize();
        _rb.AddForce(diff * (speed * _rb.mass * _rb.gravityScale));
        if (diff.x * transform.localScale.x < 0) //Ha másfelé néz, mint amerre megy
        {
            var scale = tr.localScale;
            scale.x *= -1;
            tr.localScale = scale;
        }
    }

    public void Die()
    {
        _rb.mass = 0.00001f;
        _rb.gravityScale = 0.01f;
        _rb.freezeRotation = false;
    }

    public bool IsAlive() => _rb.mass > 0.001f;
}
