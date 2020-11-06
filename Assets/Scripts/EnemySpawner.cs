using System;
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
    private void Start()
    {
        _diff = spawnPos.position - transform.position;
        enemyPrefab.GetComponent<EnemyController>().target = target;
        _pool = new ObjectPool(enemyPrefab.gameObject, 10);
    }

    private void FixedUpdate()
    {
        if (Time.fixedTime - _lastSpawn > timeBetweenSpawns && _random.Next(2) == 1)
        {
            short count = (short) _random.Next(maxEnemyCount);
            for (int i = 0; i < count; i++)
            {
                var pos = transform.position + _diff;
                var enemy = _pool.GetObject();
                enemy.transform.position = new Vector3(pos.x, spawnPos.position.y, pos.z);
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