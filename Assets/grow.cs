using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grow : MonoBehaviour
{
    private Vector3 targetSize = new Vector3(2.5f, 2.5f, 2.5f);
    private float growSpeed = 2f;
    // Start is called before the first frame update
    void Start()
    {
        this.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.localScale = Vector3.MoveTowards(this.transform.localScale, targetSize, growSpeed * Time.deltaTime);

    }
}
