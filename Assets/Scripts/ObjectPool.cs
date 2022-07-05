using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;
    public List<GameObject> PooledMissiles = new List<GameObject>();

    [SerializeField] private GameObject _missilePrefab;
    [SerializeField] private GameObject _markerPrefab;

    private int _amountOfMissilesToPool = 30;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        for (int i = 0; i < _amountOfMissilesToPool; i++)
        {
            GameObject missile = Instantiate(_missilePrefab);
            GameObject marker = Instantiate(_markerPrefab);
            missile.GetComponent<Missile>().Marker = marker;
            missile.SetActive(false);
            marker.SetActive(false);
            PooledMissiles.Add(missile);
        }


    }
    public GameObject GetPooledMissile()
    {
        for (int i = 0; i < PooledMissiles.Count; i++)
        {
            if (!PooledMissiles[i].activeInHierarchy)
            {
                return PooledMissiles[i];
            }
        }
        return null;
    }

}
