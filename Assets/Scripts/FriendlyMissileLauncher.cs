using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyMissileLauncher : MonoBehaviour, iDestroyable
{
    public TMPro.TextMeshProUGUI MissilesLeftUI;
    public int missilesLeft;

    [SerializeField] private Transform _mouseTarget;
    private float _missleSpeed = 10;

    private void OnEnable()
    {
        MissilesLeftUI.text = $"{missilesLeft}";
    }

    public void FireMissile()
    {
        GameObject missile = ObjectPool.Instance.GetPooledMissile();

        if (missile != null && gameObject.activeSelf == true && _mouseTarget.position.y > -12.7f)   // Avoids shooting the ground.
        {
            missilesLeft--;
            missile.SetActive(true);
            SetFriendlyMissileParameters(missile);

            MissilesLeftUI.text = $"{missilesLeft}";
            if (missilesLeft == 0)
            {
                gameObject.SetActive(false);
            }
        }
    }

    private void SetFriendlyMissileParameters(GameObject missile)
    {
        Missile missileScript = missile.GetComponent<Missile>();

        missileScript.Marker.transform.position = _mouseTarget.position;
        missileScript.Marker.SetActive(true);
        missileScript.Destination = _mouseTarget.position;
        missileScript.MissleSpeed = _missleSpeed;
        missileScript.SpriteRenderer.color = Color.green;
        missileScript.CirceClollider.enabled = false;   // Avoids colliding and exploding before reaching destinated position.
        missileScript.IsFriendly = true;

        // Spawn missile inside missile launcher.
        missile.transform.position = transform.position;
        float targetDirectionX = _mouseTarget.position.x - missile.transform.position.x;
        float targetDirectionY = _mouseTarget.position.y - missile.transform.position.y;
        float angle = Mathf.Atan2(targetDirectionY, targetDirectionX) * Mathf.Rad2Deg;
        missile.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 180));   // Makes missile face its destinated position.


    }

    public void Destroy()
    {
        missilesLeft = 0;
        MissilesLeftUI.text = $"{missilesLeft}";
        gameObject.SetActive(false);
    }
}
