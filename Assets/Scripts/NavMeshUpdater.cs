using System;
using System.Collections;
using Pathfinding;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(AstarPath))]
public class NavMeshUpdater : MonoBehaviour
{
    public Transform player;
    
    private AstarPath _pathfinding;
    private GridGraph _gg;

    private void Start()
    {
        _pathfinding = GetComponent<AstarPath>();
        _gg = _pathfinding.data.gridGraph;
    }

    private void Update()
    {
        if (Mathf.Abs(player.position.x - _gg.center.x) > _gg.width / 4f)
        {
            var pos = _gg.center;
            pos.x = player.position.x;
            _gg.center = pos;
            _gg.Scan();
            //StartCoroutine((IEnumerator) _pathfinding.ScanAsync());
        }
    }
}