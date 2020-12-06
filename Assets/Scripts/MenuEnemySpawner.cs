using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace DefaultNamespace
{
    public class MenuEnemySpawner : MonoBehaviour
    {
        public Rigidbody2D enemyPrefab;
        private ObjectPool _pool;
        private float _lastSpawnTime;
        private List<Rigidbody2D> _enemies = new List<Rigidbody2D>(5);
        private Random _random = new Random();
        private void Start()
        {
            enemyPrefab.mass = 0.00001f;
            enemyPrefab.gravityScale = 0.01f;
            enemyPrefab.freezeRotation = false;
            enemyPrefab.angularVelocity = 2f;
            
            _pool = new ObjectPool(enemyPrefab.gameObject, 5);
        }

        private void Update()
        {
            if (Time.time - _lastSpawnTime > 2f)
            {
                var enemy = _pool.GetObject();
                enemy.transform.position = transform.position;
                enemy.SetActive(true);
                _enemies.Add(enemy.GetComponent<Rigidbody2D>());
                _lastSpawnTime = Time.time;
            }

            for (var index = 0; index < _enemies.Count; index++)
            {
                var enemy = _enemies[index];
                enemy.AddTorque(0.11f);
                enemy.AddForce(new Vector2(((float) _random.NextDouble() * 20f + 5f) * enemy.mass * enemy.gravityScale,
                    ((float) _random.NextDouble() * 30f + 5f) * enemy.mass * enemy.gravityScale));
                if (enemy.position.x > 100 || enemy.position.y > 100)
                {
                    enemy.gameObject.SetActive(false);
                    _enemies.RemoveAt(index--);
                }
            }
        }
    }
}