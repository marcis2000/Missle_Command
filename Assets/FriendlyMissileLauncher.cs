using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyMissileLauncher : MonoBehaviour, iDestroyable
{
    public TMPro.TextMeshProUGUI missilesLeftUI;
    public int missilesLeft;

    [SerializeField] private Transform mouseTarget;
    private float missleSpeed = 10;

    private void OnEnable()
    {
        missilesLeftUI.text = $"{missilesLeft}";
    }

    public void FireMissile()
    {
        GameObject missile = ObjectPool.instance.GetPooledMissile();

        if (missile != null && gameObject.activeSelf == true && mouseTarget.position.y > -12.7f)
        {
            missilesLeft--;
            missile.SetActive(true);
            SetMissileDestination(missile);

            missilesLeftUI.text = $"{missilesLeft}";
            if (missilesLeft == 0)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void SetMissileDestination(GameObject missile)
    {
        Missile missileScript = missile.GetComponent<Missile>();

        missileScript.destination = mouseTarget.position;
        missileScript.missleSpeed = missleSpeed;
        missileScript.spriteRenderer.color = Color.green;
        missileScript.circeClollider.enabled = false;
        missileScript.isFriendly = true;
        missileScript.marker.transform.position = mouseTarget.position;
        missileScript.marker.SetActive(true);

        missile.transform.position = transform.position;
        float targetDirectionX = mouseTarget.position.x - missile.transform.position.x;
        float targetDirectionY = mouseTarget.position.y - missile.transform.position.y;
        float angle = Mathf.Atan2(targetDirectionY, targetDirectionX) * Mathf.Rad2Deg;
        missile.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 180));

    }

    public void Destroy()
    {
        missilesLeft = 0;
        missilesLeftUI.text = $"{missilesLeft}";
        gameObject.SetActive(false);
    }
}
