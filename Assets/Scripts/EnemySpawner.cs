using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class EnemySpawner : MonoBehaviour
{
    public Transform spawnPos;
    public float timeBetweenSpawns;
    public short maxEnemyCount;
    public GameObject enemyPrefab;
    public Transform target;
    private Vector3 _diff;
    private float _lastSpawn;
    private readonly Random _random = new Random();
    private ObjectPool _pool;
    private PlatformSpawner _spawner;
    private void Start()
    {
        _diff = spawnPos.position - target.position;
        enemyPrefab.GetComponent<EnemyController>().target = target;
        _pool = new ObjectPool(enemyPrefab.gameObject, 10);
        _spawner = GetComponent<PlatformSpawner>();
    }

    private void FixedUpdate()
    {
        //int timeMultiplier = _spawner.maxLevel - _spawner.Level;
        int countMultiplier = _spawner.Level;
        if (Time.fixedTime - _lastSpawn > timeBetweenSpawns && _random.Next(2) == 1)
        {
            short count = (short) _random.Next(maxEnemyCount * countMultiplier);
            for (int i = 0; i < count; i++)
            {
                var pos = target.position + _diff;
                var enemy = _pool.GetObject(true);
                if (enemy is null) break;
                enemy.transform.position = pos;
                var rb = enemy.GetComponent<Rigidbody2D>();
                rb.mass = 1f;
                rb.gravityScale = 1f;
                rb.rotation = 0f;
                rb.freezeRotation = true;
                enemy.SetActive(true);
                rb.rotation = 0f;
            }

            _lastSpawn = Time.fixedTime;
        }
    }
}