using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

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
    protected bool _isBuilding = false;
    protected Team _team;

    [SerializeField] public static float MAX_HEALTH = 25f;
    [SerializeField] public static float RANGE = 5f;
    [SerializeField] public static float BUILD_COST = 50f;
    [SerializeField] public static float BUILD_TIME = 20f;

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
        if (_isBuilding)
        {
            if (_team.poly <= 0f && _team.income < 0f)
            {
                _isBuilding = false;
                SetBuildingCost(_isBuilding);
            }
            else
            {
                progress += Time.fixedDeltaTime;
            }
        }
        else if (_unit != EUnits.None && !_isBuilding)
        {
            float bc;
            switch (_unit)
            {
                case EUnits.Cube:
                    bc = Cube.BUILD_COST;
                    break;
                case EUnits.Sphere:
                    bc = Sphere.BUILD_COST;
                    break;
                case EUnits.Tetra:
                    bc = Tetra.BUILD_COST;
                    break;
                default:
                    bc = 0f;
                    break;
            }
            if (_team.poly >= bc)
            {
                _isBuilding = true;
                SetBuildingCost(_isBuilding);
                progress += Time.fixedDeltaTime;
            }
        }
    }

    public override void Reset()
    {
        StopCmd();
        health = MAX_HEALTH;
    }

    public override float GetRange()
    {
        return RANGE;
    }

    public void MoveCmd(Vector3 position)
    {
        rallyPos = position;
    }

    public void StopCmd()
    {
        _isBuilding = false;
        SetBuildingCost(_isBuilding);
        _unit = EUnits.None;
        progress = 1f;
        maxProgress = 1f;
        rallyPos = Vector3.MoveTowards(transform.position, Vector3.zero, 5f);
    }

    protected void SetBuildingCost(bool isBuilding)
    {
        if (isBuilding)
        {
            switch (_unit)
            {
                case EUnits.Cube:
                    _team.AddExpense(Cube.BUILD_COST);
                    break;
                case EUnits.Sphere:
                    _team.AddExpense(Sphere.BUILD_COST);
                    break;
                case EUnits.Tetra:
                    _team.AddExpense(Tetra.BUILD_COST);
                    break;
                default:
                    break;
            }
        }
        else
        {
            switch (_unit)
            {
                case EUnits.Cube:
                    _team.AddExpense(-Cube.BUILD_COST);
                    break;
                case EUnits.Sphere:
                    _team.AddExpense(-Sphere.BUILD_COST);
                    break;
                case EUnits.Tetra:
                    _team.AddExpense(-Tetra.BUILD_COST);
                    break;
                default:
                    break;
            }
        }
    }

    protected Asset BuildGeneric(AssetPool assetPool)
    {
        Player owner = GetComponent<PlayerComponent>().player;
        Asset newAsset = assetPool.Get();
        newAsset.transform.position = gameObject.transform.position;
        newAsset.transform.rotation = gameObject.transform.rotation;
        newAsset.GetComponent<PlayerComponent>().player = owner;
        owner.AddPlayerAsset(newAsset);
        if (null == rallyPos)
        {
            rallyPos = Vector3.MoveTowards(transform.position, Vector3.zero, 5f);
        }
        if (newAsset is Unit newUnit)
        {
            newUnit.MoveCmd(rallyPos.Value);
        }
        return newAsset;
    }

    public void BuildCubes()
    {
        _isBuilding = false;
        SetBuildingCost(_isBuilding);
        _team = GetComponent<PlayerComponent>().player.team;
        _unit = EUnits.Cube;
        _isBuilding = true;
        SetBuildingCost(_isBuilding);
        progress = 0f;
        maxProgress = Cube.BUILD_TIME;
    }

    public void BuildSpheres()
    {
        _isBuilding = false;
        SetBuildingCost(_isBuilding);
        _team = GetComponent<PlayerComponent>().player.team;
        _unit = EUnits.Sphere;
        _isBuilding = true;
        SetBuildingCost(_isBuilding);
        progress = 0f;
        maxProgress = Sphere.BUILD_TIME;
    }

    public void BuildTetras()
    {
        _isBuilding = false;
        SetBuildingCost(_isBuilding);
        _team = GetComponent<PlayerComponent>().player.team;
        _unit = EUnits.Tetra;
        _isBuilding = true;
        SetBuildingCost(_isBuilding);
        progress = 0f;
        maxProgress = Tetra.BUILD_TIME;
    }
}
