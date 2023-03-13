using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolBase : MonoBehaviour
{
    protected RectTransform Rect => _rect;

    private RectTransform _rect;
    protected List<GameObject> PooledObjects;

    [SerializeField] protected List<GameObject> ObjectsToPool;


    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
    }
    public virtual GameObject GetPooledObject()
    {
        for (int i = 0; i < ObjectsToPool.Count; i++)
        {
            if (!PooledObjects[i].activeInHierarchy)
                return PooledObjects[i];
        }

        return null;
    }

    protected virtual void InitializePool()
    {
        PooledObjects = new List<GameObject>();
        GameObject temp;

        for (int i = 0; i < ObjectsToPool.Count; i++)
        {
            temp = Instantiate(ObjectsToPool[i]);
            temp.SetActive(false);
            PooledObjects.Add(temp);
        }
    }
}
