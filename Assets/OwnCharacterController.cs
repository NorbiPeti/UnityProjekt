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
        rb.AddForce(new Vector2(input * 5, 0));

        var cols = new Collider2D[5];
        /*if (Input.GetButtonDown("Jump") && rb.OverlapCollider(new ContactFilter2D(), cols) > 0
                                        && cols.Any(col => col.CompareTag("Tiled")))*/
        if (Input.GetButtonDown("Jump") && IsOnGround())
            rb.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
    }

    private bool IsOnGround()
    {
        var bounds = collider.bounds;
        return Physics.CheckCapsule(bounds.center, new Vector3(bounds.center.x, bounds.min.y - 0.1f, bounds.center.z),
            0.28f);
    }
}