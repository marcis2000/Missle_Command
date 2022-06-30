using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyMissileLauncherManager : MonoBehaviour
{
    [SerializeField] FriendlyMissileLauncher[] friendlyLaunchers;

    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && friendlyLaunchers[0].missilesLeft != 0)
        {
            friendlyLaunchers[0].FireMissile();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && friendlyLaunchers[1].missilesLeft != 0)
        {
            friendlyLaunchers[1].FireMissile();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && friendlyLaunchers[2].missilesLeft != 0)
        {
            friendlyLaunchers[2].FireMissile();
        }
    }
}
