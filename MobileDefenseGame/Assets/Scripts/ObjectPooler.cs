using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public GameObject pooledObject;
    public int poolCount = 28;
    public bool more = true;

    private List<GameObject> pooledObjects;

    void Start()
    {
        pooledObjects = new List<GameObject>();
        while(poolCount > 0)
        {
            GameObject obj = (GameObject)Instantiate(pooledObject);
            obj.SetActive(false);
            pooledObjects.Add(obj);
            poolCount = poolCount - 1;
            GameManager.instance.bulletAddCount++;
        }      
    }

    public GameObject GetObject()
    {
        foreach(GameObject obj in pooledObjects)
        {
            if (!obj.activeInHierarchy)
            {               
                return obj;
            }
        }
        if (more)
        {
            GameObject obj = (GameObject)Instantiate(pooledObject);
            pooledObjects.Add(obj);
            GameManager.instance.bulletAddCount++;
            return obj;
        }
        return null;
    }
}
