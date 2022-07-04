using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyMissileLauncher : MonoBehaviour
{
    public int numberOfMissileWaves;
    public float timeToNextLaunch = 5;
    public float missleSpeed = 1;
    public event EventHandler OnAddToDeLaunchedEnemyMissilesCount;


    private GameObject[] defenders;
    private Transform target;

    public void SetNewParameters()
    {
        defenders = GameObject.FindGameObjectsWithTag("Defenders");
        numberOfMissileWaves = 3;
        timeToNextLaunch += 0.5f;
        missleSpeed++;
        StartCoroutine(SpawnMissiles());
    }

    public void FireMissile(Vector3 startPosition)
    {
        GameObject missile = ObjectPool.instance.GetPooledMissile();
        if (missile != null) 
        {
            do
            {
                target = defenders[UnityEngine.Random.Range(0, defenders.Length)].transform;
            } 
            while (target.gameObject.activeSelf == false && IsAnyDefenderIsActive());

            missile.transform.position = startPosition;
            missile.SetActive(true);
            SetMissileDestination(missile);
            OnAddToDeLaunchedEnemyMissilesCount?.Invoke(this, EventArgs.Empty);

        }

    }

    public void SetMissileDestination(GameObject missile)
    {
        Missile missileScript = missile.GetComponent<Missile>();

        missileScript.destination = target.position;
        missileScript.missleSpeed = missleSpeed;
        missileScript.spriteRenderer.color = Color.red;
        missileScript.circeClollider.enabled = true;
        missileScript.isFriendly = false;
        float targetDirectionX = target.position.x - missile.transform.position.x;
        float targetDirectionY = target.position.y - missile.transform.position.y;
        float angle = Mathf.Atan2(targetDirectionY, targetDirectionX) * Mathf.Rad2Deg;
        missile.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 180));
    }

    private bool IsAnyDefenderIsActive()
    {
        bool isAnyDefenderActive = false;
        foreach(GameObject defender in defenders)
        {
            if(defender.activeSelf == true)
            {
                isAnyDefenderActive = true;
            }
        }
        return isAnyDefenderActive;


    }
    public IEnumerator SpawnMissiles()
    {
        while(numberOfMissileWaves > 0)
        {
            int i = 0;
            while (i < 4)
            {
                Vector3 startPosition = new Vector3(UnityEngine.Random.Range(-18.0f, 18.0f), 16f, 0);
                FireMissile(startPosition);
                i++;
            }
            numberOfMissileWaves--;
            yield return new WaitForSeconds(timeToNextLaunch);
        }
    }
}
