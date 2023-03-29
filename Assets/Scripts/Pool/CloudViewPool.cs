using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudViewPool : PoolBase
{
    [SerializeField]
    private float _spawnDelay;

    private float _spawnDelayCounter;

    private List<CloudView> _cloudPool;

    private void Start()
    {
        InitializePool();
        _spawnDelayCounter = _spawnDelay;
        StartCoroutine(SpawnCloud());
    }

    public override GameObject GetPooledObject()
    {
        int minPoolIndex = 0;
        int maxPoolIndex = ObjectsToPool.Count;
        int indexToGet = Random.Range(minPoolIndex, maxPoolIndex);

        if (!PooledObjects[indexToGet].activeInHierarchy)
            return PooledObjects[indexToGet];

        return null;
    }

    private IEnumerator SpawnCloud()
    {
        while (true)
        {
            yield return new WaitForSeconds(_spawnDelayCounter);

            EnableSelectedPoolObject();
        }
    }

    private void EnableSelectedPoolObject()
    {
        GameObject poolObject = GetPooledObject();
        if (poolObject != null)
        {

            poolObject.SetActive(true);
        }

    }

    protected override void InitializePool()
    {
        base.InitializePool();

        _cloudPool = new List<CloudView>();

        foreach (GameObject poolObject in PooledObjects)
            _cloudPool.Add(poolObject.GetComponent<CloudView>());

        foreach (CloudView cloud in _cloudPool)
        {
            cloud.Rect.SetParent(Rect);
            cloud.Rect.anchoredPosition3D = Vector3.zero;
            cloud.Rect.localScale = Rect.localScale;
        }
    }
}
