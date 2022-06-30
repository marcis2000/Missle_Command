using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMissileLauncher : MonoBehaviour
{
    GameObject[] defenders;
    Transform target;
    private float missleSpeed = 5;
    private float timeBetweenLaunches =2f;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        defenders = GameObject.FindGameObjectsWithTag("Defenders");
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timeBetweenLaunches)
        {
            timer = 0;
            GameObject missile = ObjectPool.instance.GetPooledObject();
            if (missile != null)
            {
                target = defenders[Random.Range(0, defenders.Length)].transform;
                if(target.gameObject.activeSelf == false)
                {
                    timer = timeBetweenLaunches;
                    return;
                }
                else
                {
                    FireMissile(missile);
                }
            }
        }
    }

    public void FireMissile(GameObject missile)
    {
        missile.SetActive(true);
        SetMissileDestination(missile);
    }

    public void SetMissileDestination(GameObject missile)
    {

        missile.GetComponent<FriendlyMissile>().destination = target.position;
        missile.GetComponent<FriendlyMissile>().missleSpeed = missleSpeed;
        missile.GetComponent<FriendlyMissile>().spriteRenderer.color = Color.red;
        missile.GetComponent<FriendlyMissile>().circeClollider.enabled = true;
        missile.transform.position = new Vector3(Random.Range(-18.0f, 18.0f), 11.5f, 0); ;
    }
}
