using UnityEngine;

public class Factory : Building
{
    protected AssetPool _cubePool;
    protected AssetPool _spherePool;
    protected AssetPool _tetraPool;

    protected override AssetPool pool { get; set; }
    protected override int health { get; set; }

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
        if (isSelected && Input.GetKeyDown(KeyCode.B))
        {
            BuildGeneric(_cubePool);
        }
        else if (isSelected && Input.GetKeyDown(KeyCode.V))
        {
            BuildGeneric(_spherePool);
        }
        else if (isSelected && Input.GetKeyDown(KeyCode.C))
        {
            BuildGeneric(_tetraPool);
        }
    }

    protected override void CheckHealth()
    {
        if (health <= 0)
        {
            pool.Release(gameObject);
        }
    }

    private void BuildGeneric(AssetPool assetPool)
    {
        Player owner = GetComponent<PlayerComponent>().player;
        GameObject newAsset = assetPool.Get();
        newAsset.transform.position = gameObject.transform.position;
        newAsset.transform.rotation = gameObject.transform.rotation;
        newAsset.GetComponent<PlayerComponent>().player = owner;
        _mainCam.GetComponent<PlayerController>().AddPlayerAsset(newAsset);
    }
}
