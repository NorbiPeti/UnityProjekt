using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OwnCharacterController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D collider;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(rb.velocity.x) > 3)
            return;
        float input = Input.GetAxis("Horizontal");
        if (input < 0 && rb.transform.localScale.x > 0
            || input > 0 && rb.transform.localScale.x < 0)
        {
            var tr = transform;
            var scale = tr.localScale;
            scale.x *= -1;
            tr.localScale = scale;
        }

        if (Input.GetButton("Fire3"))
            input *= 10;
        rb.AddForce(new Vector2(input * 5, 0));

        if (Input.GetButtonDown("Jump") && IsOnGround())
            rb.AddForce(new Vector2(0, 4), ForceMode2D.Impulse);
    }

    private bool IsOnGround()
    {
        var res = new List<Collider2D>();
        rb.OverlapCollider(new ContactFilter2D(), res);
        return res.Any(col => col.CompareTag("Ground"));
    }
}