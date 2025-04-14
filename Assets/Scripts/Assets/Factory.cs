using UnityEngine;

public class Factory : Building
{
    protected enum EUnits
    {
        Cube,
        Sphere,
        Tetra,
        None
    }

    protected AssetPool _cubePool;
    protected AssetPool _spherePool;
    protected AssetPool _tetraPool;
    protected EUnits _unit = EUnits.None;
    protected float _progress = 0f;

    protected override float MAX_HEALTH { get; } = 50f;
    public override float BUILD_COST { get; } = 50f;
    public override float BUILD_TIME { get; } = 20f;

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
        if (_unit != EUnits.None)
        {
            switch (_unit)
            {
                case EUnits.Cube:
                    {
                        if (_progress >= _cubePool._prefab.BUILD_TIME)
                        {
                            BuildGeneric(_cubePool);
                            _progress = 0f;
                        }
                        break;
                    }
                case EUnits.Sphere: 
                    {
                        if (_progress >= _spherePool._prefab.BUILD_TIME)
                        {
                            BuildGeneric(_spherePool);
                            _progress = 0f;
                        }
                        break;
                    }
                case EUnits.Tetra:
                    {
                        if (_progress >= _tetraPool._prefab.BUILD_TIME)
                        {
                            BuildGeneric(_tetraPool);
                            _progress = 0f;
                        }
                        break;
                    }
            }
            _progress += Time.fixedDeltaTime;
        }
    }

    protected Asset BuildGeneric(AssetPool assetPool)
    {
        Player owner = gameObject.GetComponent<PlayerComponent>().player;
        Asset newAsset = assetPool.Get();
        newAsset.transform.position = gameObject.transform.position;
        newAsset.transform.rotation = gameObject.transform.rotation;
        newAsset.GetComponent<PlayerComponent>().player = owner;
        owner.AddPlayerAsset(newAsset);
        if (newAsset is Unit newUnit)
        {
            newUnit.MoveCmd(Vector3.MoveTowards(transform.position, Vector3.zero, 3f));
        }
        return newAsset;
    }

    public void BuildCubes()
    {
        _progress = 0f;
        _unit = EUnits.Cube;
    }

    public void BuildSpheres()
    {
        _progress = 0f;
        _unit = EUnits.Sphere;
    }

    public void BuildTetras()
    {
        _progress = 0f;
        _unit = EUnits.Tetra;
    }
}
