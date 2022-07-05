using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Spaceship : MonoBehaviour, iDestroyable
{

    public event EventHandler OnAddSpaceshipPoints;

    [SerializeField] private Transform _leftSpawnPoint;
    [SerializeField] private Transform _rightSpawnPoint;
    [SerializeField] private EnemyMissileLauncher _launcher;

    private Vector2 _destination;
    private float _spaceshipSpeed = 2f;
    private float _timer = 0;
    private float _timeToLaunch;
    private bool _alreadyLaunched = false;

    // Draws where to spawn itself and when to fire missile.
     void OnEnable()
    {
        _alreadyLaunched = false;
        _timer = 0;
        _timeToLaunch = UnityEngine.Random.Range(2, 15);
        int num = UnityEngine.Random.Range(0, 2);
        switch (num)
        {
            case 0:
                transform.position = _leftSpawnPoint.position;
                _destination = _rightSpawnPoint.position;
                break;
            case 1:
                transform.position = _rightSpawnPoint.position;
                _destination = _leftSpawnPoint.position;
                break;
        }

    }
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _destination, _spaceshipSpeed * Time.deltaTime);
        if (!_alreadyLaunched)
        {
            _timer += Time.deltaTime;
            if (_timer >= _timeToLaunch)
            {
                _launcher.FireMissile(transform.position); // Fires missile form spaceship position.
                _alreadyLaunched = true;
            }
            if (Vector3.Distance(transform.position, _destination) < 0.001f)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void Destroy()
    {
        gameObject.SetActive(false);
        OnAddSpaceshipPoints?.Invoke(this, EventArgs.Empty);
    }
}
