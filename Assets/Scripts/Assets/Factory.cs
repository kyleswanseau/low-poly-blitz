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

    public override float MAX_HEALTH { get; } = 50f;
    public override float RANGE { get; } = 5f;
    public override float BUILD_COST { get; } = 50f;
    public override float BUILD_TIME { get; } = 20f;

    protected override AssetPool pool { get; set; }
    protected override float health { get; set; } = 50f;
    public float progress { get; private set; } = 1f;
    public float maxProgress { get; private set; } = 1f;
    public Vector3? rallyPos { get; private set; } = null;

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
        if (_unit != EUnits.None && progress >= maxProgress)
        {
            switch (_unit)
            {
                case EUnits.Cube:
                    BuildGeneric(_cubePool);
                    progress = 0f;
                    break;
                case EUnits.Sphere: 
                    BuildGeneric(_spherePool);
                    progress = 0f;
                    break;
                case EUnits.Tetra:
                    BuildGeneric(_tetraPool);
                    progress = 0f;
                    break;
            }
        }
        progress += Time.fixedDeltaTime;
    }

    public void MoveCmd(Vector3 position)
    {
        rallyPos = position;
    }

    public void StopCmd()
    {
        _unit = EUnits.None;
        progress = 1f;
        maxProgress = 1f;
        rallyPos = Vector3.MoveTowards(transform.position, Vector3.zero, 3f);
    }

    protected Asset BuildGeneric(AssetPool assetPool)
    {
        Player owner = gameObject.GetComponent<PlayerComponent>().player;
        Asset newAsset = assetPool.Get();
        newAsset.transform.position = gameObject.transform.position;
        newAsset.transform.rotation = gameObject.transform.rotation;
        newAsset.GetComponent<PlayerComponent>().player = owner;
        owner.AddPlayerAsset(newAsset);
        if (null == rallyPos)
        {
            rallyPos = Vector3.MoveTowards(transform.position, Vector3.zero, 3f);
        }
        if (newAsset is Unit newUnit)
        {
            newUnit.MoveCmd(rallyPos.Value);
        }
        return newAsset;
    }

    public void BuildCubes()
    {
        _unit = EUnits.Cube;
        progress = 0f;
        maxProgress = _cubePool._prefab.BUILD_TIME;
    }

    public void BuildSpheres()
    {
        _unit = EUnits.Sphere;
        progress = 0f;
        maxProgress = _spherePool._prefab.BUILD_TIME;
    }

    public void BuildTetras()
    {
        _unit = EUnits.Tetra;
        progress = 0f;
        maxProgress = _tetraPool._prefab.BUILD_TIME;
    }
}
