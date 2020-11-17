using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class CharacterControllerBase : MonoBehaviour
{
    protected Rigidbody2D _rb;
    
    public bool IsOnGround(string groundName = "")
    {
        var res = new List<Collider2D>();
        _rb.OverlapCollider(new ContactFilter2D(), res);
        return res.Any(col => col.CompareTag("Ground") && (groundName.Length == 0 || col.name.StartsWith(groundName)));
    }
}