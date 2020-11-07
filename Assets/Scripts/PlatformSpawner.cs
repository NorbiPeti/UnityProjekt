using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class PlatformSpawner : MonoBehaviour
{
    public Transform platformLeft;
    public Transform platformMiddle;
    public Transform platformRight;
    public Transform player;
    public int maxSize = 5;
    public int maxLevel = 5;
    public int platformCount = 2;

    private Vector3 _spawnDiff;
    private int _level;
    private Random _random = new Random();
    private Vector3 _lastPlatformPos;
    private OwnCharacterController _playerController;
    private float _lastLevel0Pos;
    private int _totalLevel;
    private int _remainingPlatforms;

    // Start is called before the first frame update
    void Start()
    {
        _spawnDiff = platformLeft.position - player.position;
        _lastPlatformPos = platformRight.position;
        _playerController = player.GetComponent<OwnCharacterController>();
        _remainingPlatforms = platformCount;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_level > 0 && player.position.x > _lastLevel0Pos && _playerController.IsOnGround("Ground"))
            _playerController.Respawn();
        if (player.position.x + _spawnDiff.x <= _lastPlatformPos.x)
            return;
        int size = _random.Next(maxSize);
        Vector3 pos = _lastPlatformPos;
        Instantiate(platformLeft).position = pos += new Vector3(1, 0, 0);
        for (int i = 0; i < size; i++)
            Instantiate(platformMiddle).position = pos += new Vector3(0.7f, 0, 0);
        Instantiate(platformRight).position = pos += new Vector3(0.7f, 0, 0);
        _lastPlatformPos = pos;
        if (_level == 0)
            _lastLevel0Pos = pos.x;
        if (--_remainingPlatforms != 0) return;
        int rand = _random.Next(2);
        switch (rand)
        {
            case 0 when _level < maxLevel:
            case 1 when _level == 0:
                _level++;
                _lastPlatformPos.y++;
                _playerController.SetCheckpoint(_lastPlatformPos);
                break;
            case 0 when _level > 1:
            case 1 when _level == maxLevel:
                _level--;
                _lastPlatformPos.y++;
                _playerController.SetCheckpoint(_lastPlatformPos);
                _lastPlatformPos.y -= 2;
                break;
        }

        _totalLevel++;
        _remainingPlatforms = (1 + _totalLevel) * platformCount;
    }
}