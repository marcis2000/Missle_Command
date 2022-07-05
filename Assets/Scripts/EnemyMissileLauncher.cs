using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyMissileLauncher : MonoBehaviour
{
    public int NumberOfMissileWaves;
    public float TimeToNextLaunch = 5;
    public float MissleSpeed = 1;
    public event EventHandler OnAddToLaunchedEnemyMissilesCount;


    private GameObject[] _defenders;
    private Transform _target;

    // Called on the beggining of every level.
    public void SetNewParameters()
    {
        NumberOfMissileWaves = 3;
        TimeToNextLaunch += 0.5f;
        MissleSpeed++;
        StartCoroutine(SpawnMissiles());
    }

    // Pick target until you find one or all defenders are not active.
    public void FireMissile(Vector3 startPosition)
    {
        GameObject missile = ObjectPool.Instance.GetPooledMissile();
        if (missile != null) 
        {
            _defenders = GameObject.FindGameObjectsWithTag("Defenders");
            do
            {
                _target = _defenders[UnityEngine.Random.Range(0, _defenders.Length)].transform;
            } 
            while (_target.gameObject.activeSelf == false && IsAnyDefenderIsActive());

            missile.transform.position = startPosition;
            missile.SetActive(true);
            SetEnemyMissileParameters(missile);
            OnAddToLaunchedEnemyMissilesCount?.Invoke(this, EventArgs.Empty);

        }

    }

    private void SetEnemyMissileParameters(GameObject missile)
    {
        Missile missileScript = missile.GetComponent<Missile>();

        missileScript.Destination = _target.position;
        missileScript.MissleSpeed = MissleSpeed;
        missileScript.SpriteRenderer.color = Color.red;
        missileScript.CirceClollider.enabled = true;
        missileScript.IsFriendly = false;

        float targetDirectionX = _target.position.x - missile.transform.position.x;
        float targetDirectionY = _target.position.y - missile.transform.position.y;
        float angle = Mathf.Atan2(targetDirectionY, targetDirectionX) * Mathf.Rad2Deg;
        missile.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 180));   // Make missile face its destinated position
    }

    // Checks if any building or missile launcher is active.
    private bool IsAnyDefenderIsActive()
    {
        bool isAnyDefenderActive = false;
        foreach(GameObject defender in _defenders)
        {
            if(defender.activeSelf == true)
            {
                isAnyDefenderActive = true;
            }
        }
        return isAnyDefenderActive;


    }

    // Fires missiles in periodic waves of four.
    // Every missile has random starting position above the camera.
    public IEnumerator SpawnMissiles()
    {
        while(NumberOfMissileWaves > 0)
        {
            int i = 0;
            while (i < 4)
            {
                Vector3 startPosition = new Vector3(UnityEngine.Random.Range(-18.0f, 18.0f), 16f, 0); 
                FireMissile(startPosition);
                i++;
            }
            NumberOfMissileWaves--;
            yield return new WaitForSeconds(TimeToNextLaunch);
        }
    }
}
