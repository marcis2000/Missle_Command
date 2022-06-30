using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyMissileLauncher : MonoBehaviour, iDestroyable
{
    [SerializeField] Transform mouseTarget;
    private float missleSpeed = 5;
    public int missilesLeft = 10;

    // Update is called once per frame
    //void Update()
    //{
       // if (mouseTarget.position.y > -9 && Input.GetKeyDown(KeyCode.Mouse0))
    public void FireMissile()
        {
            GameObject missile = ObjectPool.instance.GetPooledObject();

            if(missile != null)
            {
                missilesLeft--;
                 missile.SetActive(true);
                SetMissileDestination(missile);
                if (missilesLeft == 0)
                {
                    gameObject.SetActive(false);
                }
            }
        }
   // }


    public void SetMissileDestination(GameObject missile)
    {
        missile.GetComponent<FriendlyMissile>().destination = mouseTarget.position;
        missile.GetComponent<FriendlyMissile>().missleSpeed = missleSpeed;
        missile.GetComponent<FriendlyMissile>().spriteRenderer.color = Color.green;
        missile.GetComponent<FriendlyMissile>().circeClollider.enabled = false;
        missile.transform.position = transform.position;
    }

    public void Destroy()
    {
        gameObject.SetActive(false);

    }
}
