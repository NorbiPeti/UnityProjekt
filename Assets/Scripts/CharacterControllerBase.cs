using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class CharacterControllerBase : MonoBehaviour
{
    protected Rigidbody2D _rb;

    public Transform foot;
    public float groundRadius;
    public LayerMask groundMask;
    
    public bool IsOnGround(string groundName = "")
    {
        var collider = Physics2D.OverlapCircle(foot.position, groundRadius, groundMask);
        return collider && (groundName.Length == 0 || collider.name.StartsWith(groundName));
    }
}