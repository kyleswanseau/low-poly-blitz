using UnityEngine;

public class Factory : Building
{
    protected AssetPool _cubePool;
    protected AssetPool _spherePool;
    protected AssetPool _tetraPool;

    protected override float MAX_HEALTH { get; } = 50f;

    protected override AssetPool pool { get; set; }
    protected override float health { get; set; } = 50f;

    protected override void Start()
    {
        base.Start();
        pool = GameObject.FindWithTag("FactoryPool").GetComponent<AssetPool>();
        _cubePool = GameObject.FindWithTag("CubePool").GetComponent<AssetPool>();
        _spherePool = GameObject.FindWithTag("SpherePool").GetComponent<AssetPool>();
        _tetraPool = GameObject.FindWithTag("TetraPool").GetComponent<AssetPool>();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected void BuildGeneric(AssetPool assetPool)
    {
        Player owner = gameObject.GetComponent<PlayerComponent>().player;
        Asset newAsset = assetPool.Get();
        newAsset.transform.position = gameObject.transform.position;
        newAsset.transform.rotation = gameObject.transform.rotation;
        newAsset.GetComponent<PlayerComponent>().player = owner;
        owner.AddPlayerAsset(newAsset);
    }

    public void BuildCube()
    {
        BuildGeneric(_cubePool);
    }

    public void BuildSphere()
    {
        BuildGeneric(_spherePool);
    }

    public void BuildTetra()
    {
        BuildGeneric(_tetraPool);
    }
}
