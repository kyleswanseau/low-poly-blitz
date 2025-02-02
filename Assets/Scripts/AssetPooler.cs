using UnityEngine;

public abstract class AssetPooler : MonoBehaviour
{
    protected bool? CollectionCheck { get; set; }
    protected int? DefaultSize { get; set; }
    protected int? MaxSize { get; set; }

    protected abstract void Awake();
    public abstract GameObject PoolAdd();
    public abstract void PoolPop();
    public abstract void PoolPush();
    public abstract void PoolDestroy();
}
