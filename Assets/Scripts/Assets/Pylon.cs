using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pylon : Building
{
    protected AssetPool _factoryPool;
    protected AssetPool _pylonPool;
    protected AssetPool _minePool;

    [SerializeField] public static float MAX_HEALTH = 15f;
    [SerializeField] public static float RANGE = 20f;
    [SerializeField] public static float BUILD_COST = 10f;
    [SerializeField] public static float BUILD_TIME = 5f;

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

    public override void Reset()
    {
        health = MAX_HEALTH;
    }

    public override float GetRange()
    {
        return RANGE;
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
        Team team = GetComponent<PlayerComponent>().player.team;
        if (team.poly >= Factory.BUILD_COST && !HasNearbyBuildings(position, Factory.RANGE))
        {
            team.AddPoly(-Factory.BUILD_COST);
            position += new Vector3(0f, 0.05f, 0f);
            BuildGeneric(_factoryPool, position);
        }
    }

    public void BuildPylon(Vector3 position)
    {
        Team team = GetComponent<PlayerComponent>().player.team;
        if (team.poly >= Pylon.BUILD_COST)
        {
            team.AddPoly(-Pylon.BUILD_COST);
            position += new Vector3(0f, 3.75f, 0f);
            BuildGeneric(_pylonPool, position);
        }
    }

    public void BuildMine(Vector3 position)
    {
        Team team = GetComponent<PlayerComponent>().player.team;
        if (team.poly >= Mine.BUILD_COST && !HasNearbyBuildings(position, Mine.RANGE))
        {
            team.AddPoly(-Mine.BUILD_COST);
            position += new Vector3(0f, 1f, 0f);
            BuildGeneric(_minePool, position);
            team.AddIncome(Mine.INCOME);
        }
    }

    protected bool HasNearbyBuildings(Vector3 pos, float range)
    {
        List<Building> buildings = new List<Building>();
        Collider[] hitColliders = Physics.OverlapSphere(pos, range);
        foreach (Collider hitCollider in hitColliders)
        {
            GameObject assetObj = hitCollider.gameObject;
            if (assetObj.GetComponent<Building>() && !assetObj.GetComponent<Pylon>())
            {
                buildings.Add(assetObj.GetComponent<Building>());
            }
        }
        return buildings.Any();
    }
}
