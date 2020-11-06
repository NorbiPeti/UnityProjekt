using System;
using UnityEngine;
using Random = System.Random;

public class EnemySpawner : MonoBehaviour
{
    public Transform spawnPos;
    public float timeBetweenSpawns;
    public short maxEnemyCount;
    public Transform enemyPrefab;
    public Transform target;
    private Vector3 _diff;
    private float _lastSpawn;
    private readonly Random _random = new Random();
    private void Start()
    {
        _diff = spawnPos.position - transform.position;
        enemyPrefab.GetComponent<EnemyController>().target = target;
    }

    private void FixedUpdate()
    {
        if (Time.fixedTime - _lastSpawn > timeBetweenSpawns && _random.Next(2) == 1)
        {
            short count = (short) _random.Next(maxEnemyCount);
            for (int i = 0; i < count; i++)
            {
                var pos = transform.position + _diff;
                Instantiate(enemyPrefab).position = new Vector3(pos.x, spawnPos.position.y, pos.z);
            }

            _lastSpawn = Time.fixedTime;
        }
    }
}