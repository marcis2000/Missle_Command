using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour, iDestroyable
{
    [SerializeField] Transform mouseTarget;
    [SerializeField] FriendlyMissle missle;
    public float missleSpeed = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (mouseTarget.position.y > -9)
            {
                missle.SetMissleDestination(mouseTarget.position, missleSpeed);

            }
        }
    }
}
