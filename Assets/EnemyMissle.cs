using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMissle : MonoBehaviour
{
    private float rotation;

    void OnEnable()
    {
        FireMissle();
    }

    void Update()
    {
        
    }

    public void FireMissle()
    {
        transform.position = new Vector2(Random.Range(-18.0f, 18.0f), 11.5f);
        Vector3 rotationVector = new Vector3(0, 0 ,Random.Range(30f, 120f));
        transform.rotation = Quaternion.Euler(rotationVector);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, rotationVector);
        if(hit.collider.tag == "Wall")
        {
            FireMissle();
        }
        else
        {
            Debug.Log("Ready to shoot");
        }
    }
}
