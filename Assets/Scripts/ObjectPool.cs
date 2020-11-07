using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    public ObjectPool(GameObject prefab, int initialSize)
    {
        _prefab = prefab;
        _objects = new List<GameObject>(initialSize);
        for (int i = 0; i < initialSize; i++)
            _objects.Add(Object.Instantiate(_prefab));
    }

    private List<GameObject> _objects;
    private GameObject _prefab;

    /// <summary>
    /// Visszaad egy új objektumot. Aktiválandó, használat után pedig deaktiválandó.
    /// </summary>
    /// <returns>Egy objektum a poolból.</returns>
    public GameObject GetObject(bool fixedPool = false)
    {
        GameObject theRocket = null;
        foreach (var rocket in _objects)
        {
            if (!rocket.activeSelf)
            {
                theRocket = rocket;
                break;
            }
        }

        if (theRocket is null)
            if (fixedPool)
                return null;
            else
                _objects.Add(theRocket = Object.Instantiate(_prefab));
        return theRocket;
    }
}