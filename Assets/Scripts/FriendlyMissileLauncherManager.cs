using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyMissileLauncherManager : MonoBehaviour
{
     public List<FriendlyMissileLauncher> FriendlyLaunchers = new List<FriendlyMissileLauncher>();

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && FriendlyLaunchers[0].missilesLeft != 0)
        {
            FriendlyLaunchers[0].FireMissile();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && FriendlyLaunchers[1].missilesLeft != 0)
        {
            FriendlyLaunchers[1].FireMissile();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && FriendlyLaunchers[2].missilesLeft != 0)
        {
            FriendlyLaunchers[2].FireMissile();
        }
    }

    public void ActivateMissileLaunchers()
    {
        foreach (FriendlyMissileLauncher missileLauncher in FriendlyLaunchers)
        {
            missileLauncher.missilesLeft = 10;
            missileLauncher.MissilesLeftUI.text = $"{missileLauncher.missilesLeft}";
            missileLauncher.gameObject.SetActive(true);
        }
    }
}
