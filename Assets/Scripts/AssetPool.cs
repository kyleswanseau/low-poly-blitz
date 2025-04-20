using UnityEngine;
using UnityEngine.Pool;

public class AssetPool : MonoBehaviour
{
    [SerializeField] private Asset _prefab;

    protected ObjectPool<Asset> _pool;
    protected bool _collectionCheck = true;
    protected int _defaultSize = 50;
    protected int _maxSize = 50;

    protected void Awake()
    {
        _pool = new ObjectPool<Asset>(
            PoolAdd, 
            PoolPop, 
            PoolPush, 
            PoolDestroy, 
            _collectionCheck, 
            _defaultSize, 
            _maxSize
            );
    }

    private Asset PoolAdd()
    {
        Asset newObject = Instantiate(_prefab);
        return newObject;
    }

    private void PoolPop(Asset obj)
    {
        obj.gameObject.SetActive(true);
    }

    private void PoolPush(Asset obj)
    {
        obj.gameObject.SetActive(false);
    }

    private void PoolDestroy(Asset obj)
    {
        Destroy(obj.gameObject);
    }

    public Asset Get()
    {
        return _pool.Get();
    }

    public void Release(Asset obj)
    {
        obj.Reset();
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
