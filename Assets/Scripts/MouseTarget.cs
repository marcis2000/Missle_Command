using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Gets 2D mouse position.

public class MouseTarget : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;

    void Update()
    {
        Vector3 mouseWorldPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0f;
        transform.position = mouseWorldPosition;
    }
  
}
