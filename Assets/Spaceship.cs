using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Spaceship : MonoBehaviour, iDestroyable
{

    public event EventHandler OnAddSpaceshipPoints;

    [SerializeField] private Transform leftSpawnPoint;
    [SerializeField] private Transform rightSpawnPoint;
    [SerializeField] private EnemyMissileLauncher launcher;

    private Vector2 destination;
    private float spaceshipSpeed = 2f;
    private float timer = 0;
    private float timeToLaunch;
    private bool alreadyLaunched = false;


    private void OnEnable()
    {
        alreadyLaunched = false;
        timer = 0;
        timeToLaunch = UnityEngine.Random.Range(2, 15);
        int num = UnityEngine.Random.Range(0, 2);
        switch (num)
        {
            case 0:
                transform.position = leftSpawnPoint.position;
                destination = rightSpawnPoint.position;
                break;
            case 1:
                transform.position = rightSpawnPoint.position;
                destination = leftSpawnPoint.position;
                break;
        }

    }
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, spaceshipSpeed * Time.deltaTime);
        if (!alreadyLaunched)
        {
            timer += Time.deltaTime;
            if (timer >= timeToLaunch)
            {
                launcher.FireMissile(transform.position);
                alreadyLaunched = true;
            }
            if (Vector3.Distance(transform.position, destination) < 0.001f)
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
