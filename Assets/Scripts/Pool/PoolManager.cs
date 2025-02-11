using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    private Dictionary<PoolableObject, List<PoolableObject>> usablePool = new Dictionary<PoolableObject, List<PoolableObject>>();

    [SerializeField] private Transform poolablesHolder;

    public static T InstantiatePoolableAtPosition<T>(T poolableObjectToInstantiate, Vector2 instancePosition) where T : PoolableObject
    {
        T pooledObject = null;

        if (!Instance.usablePool.ContainsKey(poolableObjectToInstantiate))
        {
            Instance.usablePool.Add(poolableObjectToInstantiate , new List<PoolableObject>());
        }

        if (Instance.usablePool[poolableObjectToInstantiate].Count == 0)
        {
            pooledObject = Instantiate(poolableObjectToInstantiate, Instance.poolablesHolder, true);
        }
        else
        {
            pooledObject = Instance.usablePool[poolableObjectToInstantiate][0] as T;
            Instance.usablePool[poolableObjectToInstantiate].RemoveAt(0);
        }

        pooledObject.transform.position = instancePosition;
        pooledObject.transform.rotation = Quaternion.identity;

        pooledObject.Initialize(poolableObjectToInstantiate);
        return pooledObject;
    }

    public static void DisablePoolableObject(PoolableObject objectToDisable)
    {
        if (!Instance.usablePool.ContainsKey(objectToDisable.PoolPrefab))
        {
            Instance.usablePool.Add(objectToDisable.PoolPrefab, new List<PoolableObject>());
        }

        Instance.usablePool[objectToDisable.PoolPrefab].Add(objectToDisable);
    }
}
