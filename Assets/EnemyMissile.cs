using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMissile : MonoBehaviour
{
    private Vector3 localPosition;
    private Vector2 localRotation;

    void Update()
    {

    }

    public void FireMissle(Vector3 destination, float speed)
    {
        localPosition = new Vector3(Random.Range(-18.0f, 18.0f), 11.5f,0);
        transform.position = localPosition;

        transform.rotation = Quaternion.Euler(localRotation);

    }
}
