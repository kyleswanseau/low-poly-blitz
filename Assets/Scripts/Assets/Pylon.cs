using UnityEngine;

public class Pylon : Building
{
    protected AssetPool _factoryPool;
    protected AssetPool _pylonPool;
    protected AssetPool _minePool;

    protected override float MAX_HEALTH { get; } = 20f;
    protected float RANGE { get; } = 10f;
    public override float BUILD_COST { get; } = 10f;
    public override float BUILD_TIME { get; } = 5f;

    protected override AssetPool pool { get; set; }
    protected override float health { get; set; } = 20f;

    protected override void Start()
    {
        base.Start();
        pool = GameObject.FindWithTag("PylonPool").GetComponent<AssetPool>();
        _factoryPool = GameObject.FindWithTag("FactoryPool").GetComponent<AssetPool>();
        _pylonPool = GameObject.FindWithTag("PylonPool").GetComponent<AssetPool>();
        _minePool = GameObject.FindWithTag("MinePool").GetComponent<AssetPool>();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected Asset BuildGeneric(AssetPool assetPool, Vector3 position)
    {
        Player owner = gameObject.GetComponent<PlayerComponent>().player;
        Asset newAsset = assetPool.Get();
        newAsset.transform.position = position;
        newAsset.GetComponent<PlayerComponent>().player = owner;
        owner.AddPlayerAsset(newAsset);
        return newAsset;
    }

    public void BuildFactory(Vector3 position)
    {
        BuildGeneric(_factoryPool, position);
    }

    public void BuildPylon(Vector3 position)
    {
        position += new Vector3(0f, 3.75f, 0f);
        BuildGeneric(_pylonPool, position);
    }

    public void BuildMine(Vector3 position)
    {
        BuildGeneric(_minePool, position);
    }
}
