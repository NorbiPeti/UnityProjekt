using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Seeker))]
public class EnemyAI : MonoBehaviour
{
    // What to chase?
    private Transform _target;

    // How many times each second we will update our path
    public float updateRate = 2f;

    // Caching
    private Seeker _seeker;
    private Rigidbody2D _rb;

    //The calculated path
    private Path _path;

    //The AI's speed per second
    public float speed = 30f;
    public ForceMode2D fMode;

    [HideInInspector] public bool pathIsEnded;

    // The max distance from the AI to a waypoint for it to continue to the next waypoint
    public float nextWaypointDistance = 2;

    // The waypoint we are currently moving towards
    private int _currentWaypoint;

    private EnemyController _controller;

    // Start is called before the first frame update
    void Start()
    {
        _seeker = GetComponent<Seeker>();
        _rb = GetComponent<Rigidbody2D>();
        _controller = GetComponent<EnemyController>();
        _target = _controller.target;

        if (_target == null)
        {
            Debug.LogError("No Player found? PANIC!");
            return;
        }

        // Start a new path to the target position, return the result to the OnPathComplete method
        _seeker.StartPath(transform.position, _target.position, OnPathComplete);

        StartCoroutine(UpdatePath());
    }

    IEnumerator UpdatePath()
    {
        // Start a new path to the target position, return the result to the OnPathComplete method
        _seeker.StartPath(transform.position, _target.position, OnPathComplete);

        yield return new WaitForSeconds(1f / updateRate);
        if (_controller.IsAlive())
            StartCoroutine(UpdatePath());
    }

    public void OnPathComplete(Path p)
    {
        Debug.Log("We got a path. Did it have an error? " + p.error);

        if (!p.error)
        {
            _path = p;
            _currentWaypoint = 0;
        }
    }

    void FixedUpdate()
    {
        if (_path == null)
            return;

        if (!_controller.IsAlive())
            return;

        if (_currentWaypoint >= _path.vectorPath.Count)
        {
            if (pathIsEnded)
                return;

            Debug.Log("End of path reached.");
            pathIsEnded = true;
            return;
        }

        pathIsEnded = false;

        //Direction to the next waypoint
        Vector3 dir = (_path.vectorPath[_currentWaypoint] - transform.position).normalized;
        dir *= speed * Time.fixedDeltaTime;

        //if(Vector3.Distance (transform.position, _target.position) < 10)
        //{
        //Move the AI
        //_rb.AddForce(dir, fMode);
        _rb.velocity = dir;
        //}

        if (_path.vectorPath[_currentWaypoint].y - transform.position.y > 1)
            _rb.AddForce(new Vector2(0, 10f));

        float dist = Vector3.Distance(transform.position, _path.vectorPath[_currentWaypoint]);
        if (dist < nextWaypointDistance)
        {
            _currentWaypoint++;
        }
    }
}