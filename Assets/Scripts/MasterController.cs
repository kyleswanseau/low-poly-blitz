using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MasterController : MonoBehaviour
{
    [SerializeField] private Asset cube;
    [SerializeField] private Asset sphere;
    [SerializeField] private Asset tetra;
    [SerializeField] private Asset factory;
    [SerializeField] private Asset mine;
    [SerializeField] private Asset pylon;
    [SerializeField] private BuildingAI buildingAI;
    [SerializeField] private UnitAI unitAI;
    private TeamController teamController;

    private void Start()
    {
        TrainBuildingAI();
        //TrainUnitAI();
    }

    private void FixedUpdate()
    {
        if (Time.fixedTime + Time.fixedDeltaTime > Mathf.Ceil(Time.fixedTime))
        {
            foreach (var kvp in teamController.GetTeams())
            {
                if (kvp.Value.index > 0)
                {
                    bool hasBuildings = false;
                    foreach (var kvp2 in kvp.Value.GetPlayers())
                    {
                        hasBuildings = kvp2.Value.assets.Any(asset => asset is Building);
                    }
                    if (!hasBuildings)
                    {
                        Debug.Log("Team " + kvp.Value.index + " has lost!");
                    }
                    kvp.Value.AddPoly(kvp.Value.income - kvp.Value.expense);
                }
            }
        }
    }

    public void TrainBuildingAI()
    {
        teamController = GetComponent<TeamController>();
        Player player1 = teamController.GetPlayer(1);
        Player player2 = teamController.GetPlayer(2);
        AssetPool pylonPool = GameObject.FindWithTag("PylonPool").GetComponent<AssetPool>();

        Asset player1Pylon = pylonPool.Get();
        player1Pylon.transform.position = new Vector3(-40f, 3.75f, -40f);
        player1Pylon.GetComponent<PlayerComponent>().player = player1;
        player1Pylon.Reset();
        player1.AddPlayerAsset(player1Pylon);

        Asset player2Pylon = pylonPool.Get();
        player2Pylon.transform.position = new Vector3(40f, 3.75f, 40f);
        player2Pylon.GetComponent<PlayerComponent>().player = player2;
        player2Pylon.Reset();
        player2.AddPlayerAsset(player2Pylon);

        foreach (var team in teamController.GetTeams())
        {
            team.Value.AddIncome(5f);
        }
    }

    public void RestartBuildingTraining()
    {
        foreach (var kvp in teamController.GetTeams())
        {
            foreach (var kvp2 in kvp.Value.GetPlayers())
            {
                List<Asset> assets = kvp2.Value.assets;
                for (int i = assets.Count - 1; i >= 0; i--)
                {
                    Asset asset = assets[i];
                    asset.Die();
                }
            }
            kvp.Value.AddPoly(-kvp.Value.poly);
            kvp.Value.AddIncome(-kvp.Value.income);
            kvp.Value.AddExpense(-kvp.Value.expense);
        }
        TrainBuildingAI();
    }

    public void TrainUnitAI()
    {
        teamController = GetComponent<TeamController>();
        Player player1 = teamController.GetPlayer(1);
        Player player2 = teamController.GetPlayer(2);
        AssetPool factoryPool = GameObject.FindWithTag("FactoryPool").GetComponent<AssetPool>();

        Asset player1Factory1 = factoryPool.Get();
        player1Factory1.transform.position = new Vector3(-40f, 0.05f, -40f);
        player1Factory1.GetComponent<PlayerComponent>().player = player1;
        player1Factory1.Reset();
        player1.AddPlayerAsset(player1Factory1);
        ((Factory)player1Factory1).BuildCubes();

        Asset player1Factory2 = factoryPool.Get();
        player1Factory2.transform.position = new Vector3(-35f, 0.05f, -40f);
        player1Factory2.GetComponent<PlayerComponent>().player = player1;
        player1Factory2.Reset();
        player1.AddPlayerAsset(player1Factory2);
        ((Factory)player1Factory2).BuildCubes();

        float xRand = Random.Range(0f, 45f);
        float yRand = Random.Range(0f, 45f);
        Asset player2Factory1 = factoryPool.Get();
        player2Factory1.transform.position = new Vector3(xRand, 0.05f, yRand);
        player2Factory1.GetComponent<PlayerComponent>().player = player2;
        player2Factory1.Reset();
        player2.AddPlayerAsset(player2Factory1);
        ((Factory)player2Factory1).BuildCubes();

        /*
        Asset player2Factory2 = factoryPool.Get();
        player2Factory2.transform.position = new Vector3(35f, 0.05f, 40f);
        player2Factory2.GetComponent<PlayerComponent>().player = player2;
        player2Factory2.Reset();
        player2.AddPlayerAsset(player2Factory2);
        ((Factory)player2Factory2).BuildCubes();

        Asset player2Factory3 = factoryPool.Get();
        player2Factory3.transform.position = new Vector3(-40f, 0.05f, 40f);
        player2Factory3.GetComponent<PlayerComponent>().player = player2;
        player2Factory3.Reset();
        player2.AddPlayerAsset(player2Factory3);
        ((Factory)player2Factory3).BuildCubes();
        */

        foreach (var team in teamController.GetTeams())
        {
            team.Value.AddIncome(20f);
        }
    }

    public void RestartUnitTraining()
    {
        foreach (var kvp in teamController.GetTeams())
        {
            foreach (var kvp2 in kvp.Value.GetPlayers())
            {
                List<Asset> assets = kvp2.Value.assets;
                for (int i = assets.Count - 1; i >= 0; i--)
                {
                    Asset asset = assets[i];
                    asset.Die();
                }
            }
            kvp.Value.AddPoly(-kvp.Value.poly);
            kvp.Value.AddIncome(-kvp.Value.income);
            kvp.Value.AddExpense(-kvp.Value.expense);
        }
        TrainUnitAI();
    }
}
