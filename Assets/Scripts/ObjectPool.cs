using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;
    public List<GameObject> pooledMissiles = new List<GameObject>();

    [SerializeField] private GameObject missilePrefab;
    [SerializeField] private GameObject markerPrefab;

    private int amountOfMissilesToPool = 30;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        for (int i = 0; i < amountOfMissilesToPool; i++)
        {
            GameObject missile = Instantiate(missilePrefab);
            GameObject marker = Instantiate(markerPrefab);
            missile.GetComponent<Missile>().marker = marker;
            missile.SetActive(false);
            marker.SetActive(false);
            pooledMissiles.Add(missile);
        }


    }
    public GameObject GetPooledMissile()
    {
        for (int i = 0; i < pooledMissiles.Count; i++)
        {
            if (!pooledMissiles[i].activeInHierarchy)
            {
                return pooledMissiles[i];
            }
        }
        return null;
    }

}
