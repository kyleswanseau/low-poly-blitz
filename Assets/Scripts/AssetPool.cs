using System;
using UnityEngine;
using UnityEngine.Pool;

public class AssetPool : MonoBehaviour
{
    public GameObject _prefab;

    protected ObjectPool<GameObject> _pool;
    protected bool _collectionCheck = true;
    protected int _defaultSize = 10;
    protected int _maxSize = 10;

    protected void Awake()
    {
        _pool = new ObjectPool<GameObject>(
            PoolAdd, 
            PoolPop, 
            PoolPush, 
            PoolDestroy, 
            _collectionCheck, 
            _defaultSize, 
            _maxSize
            );
    }

    private GameObject PoolAdd()
    {
        GameObject newObject = Instantiate(_prefab);
        return newObject;
    }

    private void PoolPop(GameObject obj)
    {
        obj.SetActive(true);
    }

    private void PoolPush(GameObject obj)
    {
        obj.SetActive(false);
    }

    private void PoolDestroy(GameObject obj)
    {
        Destroy(obj.gameObject);
    }

    public GameObject Get()
    {
        return _pool.Get();
    }

    public void Release(GameObject obj)
    {
        _pool.Release(obj);
    }

    public void Clear()
    {
        _pool.Clear();
    }

    public void Dispose()
    {
        _pool.Dispose();
    }

    public int CountAll()
    {
        return _pool.CountAll;
    }

    public int CountActive()
    {
        return _pool.CountActive;
    }

    public int CountInactive()
    {
        return _pool.CountInactive;
    }
}
