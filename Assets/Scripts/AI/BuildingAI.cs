using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class BuildingAI : Agent
{
    [SerializeField] private int allyID;
    [SerializeField] private int enemyID;
    [SerializeField] private MasterController _masterController;
    [SerializeField] private TeamController _teamController;
    private Team _team;
    private Player _player;
    private Player _enemy;
    private System.Random _rng = new System.Random();

    private void Start()
    {
        if (allyID == 0)
        {
            allyID = 2;
        }
        if (enemyID == 0)
        {
            enemyID = 1;
        }
        _team = _teamController.GetTeam(allyID);
        _player = _team.GetPlayers().Values.First();
        _enemy = _teamController.GetTeam(enemyID).GetPlayers().Values.First();
    }

    private void FixedUpdate()
    {
        if (Time.fixedTime + Time.fixedDeltaTime > Mathf.Ceil(Time.fixedTime))
        {
            ApplyReward();
        }
    }

    public override void OnEpisodeBegin()
    {
        if (Time.time > 0f)
        {
            _masterController.RestartBuildingTraining();
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        IEnumerable<Asset> buildings = _player.assets.Where(a => a is Building);
        IEnumerable<Asset> alliedUnits = _player.assets.Where(a => a is Unit);
        IEnumerable<Asset> enemyUnits = _enemy.assets.Where(a => a is Unit);
        sensor.AddObservation(_team.poly);
        sensor.AddObservation(_team.income);
        sensor.AddObservation(_team.expense);
        sensor.AddObservation(buildings.Where(a => a is Factory).Count());
        sensor.AddObservation(buildings.Where(a => a is Pylon).Count());
        sensor.AddObservation(buildings.Where(a => a is Mine).Count());
        sensor.AddObservation(alliedUnits.Where(a => a is Cube).Count());
        sensor.AddObservation(enemyUnits.Where(a => a is Cube).Count());
        sensor.AddObservation(alliedUnits.Where(a => a is Sphere).Count());
        sensor.AddObservation(enemyUnits.Where(a => a is Sphere).Count());
        sensor.AddObservation(alliedUnits.Where(a => a is Tetra).Count());
        sensor.AddObservation(enemyUnits.Where(a => a is Tetra).Count());
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        switch(actions.DiscreteActions[0])
        {
            case 1:
                if (_team.poly >= Factory.BUILD_COST)
                {
                    BuildFactory(actions.DiscreteActions[1]);
                }
                break;
            case 2:
                if (_team.poly >= Pylon.BUILD_COST)
                {
                    BuildPylon();
                }
                break;
            case 3:
                if (_team.poly >= Mine.BUILD_COST)
                {
                    BuildMine();
                }
                break;
            default:
                break;
        }
    }

    public void ApplyReward()
    {
        IEnumerable<Asset> buildings = _player.assets.Where(a => a is Building);
        IEnumerable<Asset> alliedUnits = _player.assets.Where(a => a is Unit);
        IEnumerable<Asset> enemyUnits = _enemy.assets.Where(a => a is Unit);
        int alliedCubes = alliedUnits.Where(a => a is Cube).Count();
        int enemyCubes = enemyUnits.Where(a => a is Cube).Count();
        int alliedSpheres = alliedUnits.Where(a => a is Sphere).Count();
        int enemySpheres = enemyUnits.Where(a => a is Sphere).Count();
        int alliedTetras = alliedUnits.Where(a => a is Tetra).Count();
        int enemyTetras = enemyUnits.Where(a => a is Tetra).Count();
        float reward = 0f;

        if (0 < _team.poly)
        {
            reward += _team.poly * 0.01f;
        }
        else if (_team.poly < 1000f)
        {
            reward += _team.poly * 0.0001f;
        }
        else
        {
            reward -= _team.poly * 0.0002f;
        }

        if (0 < _team.income)
        {
            reward +=_team.income * 0.002f;
        }
        else
        {
            reward += _team.income * 0.001f;
        }

        if (_team.income <= _team.expense)
        {
            reward += (_team.income - _team.expense) * 0.005f;
        }
        else if (_team.income <= _team.expense + 20f)
        {
            reward += (_team.income - _team.expense) * 0.01f;
        }
        else
        {
            reward -= (_team.income - _team.expense) * 0.02f;
        }

        reward += buildings.Where(a => a is Factory f && f.isBuilding == true).Count() * 0.02f;
        if (buildings.Where(a => a is Pylon).Count() >
            buildings.Where(a => a is Factory).Count() + buildings.Where(a => a is Mine).Count())
        {
            reward -= 0.2f;
        }
        if (buildings.Where(a => a is Pylon).Count() >
            buildings.Where(a => a is Factory).Count() + buildings.Where(a => a is Mine).Count() + 5)
        {
            reward -= 10f;
        }

        reward += _player.assets.Where(a => a is Unit).Count() * 0.01f;
        if (alliedTetras > 0 && (enemyCubes / alliedTetras <
            1 / ((Cube.BUILD_COST * Cube.BUILD_TIME) / (Tetra.BUILD_COST * Tetra.BUILD_TIME))))
        {
            reward -= 0.2f;
        }
        if (alliedCubes > 0 && (enemySpheres / alliedCubes <
            1 / ((Sphere.BUILD_COST * Sphere.BUILD_TIME) / (Cube.BUILD_COST * Cube.BUILD_TIME))))
        {
            reward -= 0.2f;
        }
        if (alliedSpheres > 0 && (enemyTetras / alliedSpheres <
            1 / ((Tetra.BUILD_COST * Tetra.BUILD_TIME) / (Sphere.BUILD_COST * Sphere.BUILD_TIME))))
        {
            reward -= 0.2f;
        }

        if (reward > 1f)
        {
            reward = 1f;
        }
        else if (reward < -1f)
        {
            reward = -1f;
        }
        SetReward(reward);
        if (reward >= 1f)
        {
            //Debug.Log("BuildingAI: Passed");
            EndEpisode();
        }
        else if (reward <= -1f)
        {
            //Debug.Log("BuildingAI: Failed");
            EndEpisode();
        }
    }

    private void BuildFactory(int unit)
    {
        IEnumerable<Pylon> pylons = _player.assets.Where(a => a is Pylon).Cast<Pylon>();
        int retry = 0;
        do
        {
            Pylon pylon = pylons.ElementAtOrDefault(_rng.Next(pylons.Count()));
            Vector2 randomCircle = UnityEngine.Random.insideUnitCircle * pylon.GetRange();
            Vector3 pos = new Vector3(pylon.transform.position.x + randomCircle.x, 0f, pylon.transform.position.z + randomCircle.y);
            if (-50f < pos.x && pos.x < 50f && -50f < pos.z && pos.z < 50f)
            {
                Factory? factory = pylon.BuildFactory(pos);
                if (null != factory)
                {
                    switch (unit)
                    {
                        case 0:
                            factory.BuildCubes();
                            break;
                        case 1:
                            factory.BuildSpheres();
                            break;
                        case 2:
                            factory.BuildTetras();
                            break;
                        default:
                            break;
                    }
                }
                AddReward(0.02f);
                retry = 10;
            }
            else
            {
                retry++;
            }
        }
        while (retry < 10);
    }

    private void BuildPylon()
    {
        IEnumerable<Pylon> pylons = _player.assets.Where(a => a is Pylon).Cast<Pylon>();
        int retry = 0;
        do
        {
            Pylon pylon = pylons.ElementAtOrDefault(_rng.Next(pylons.Count()));
            Vector2 randomCircle = UnityEngine.Random.insideUnitCircle * pylon.GetRange();
            Vector3 pos = new Vector3(pylon.transform.position.x + randomCircle.x, 0f, pylon.transform.position.z + randomCircle.y);
            if (-50f < pos.x && pos.x < 50f && -50f < pos.z && pos.z < 50f)
            {
                pylon.BuildPylon(pos);
                AddReward(-0.5f);
                retry = 10;
            }
            else
            {
                retry++;
            }
        }
        while (retry < 10);
    }

    private void BuildMine()
    {
        IEnumerable<Pylon> pylons = _player.assets.Where(a => a is Pylon).Cast<Pylon>();
        int retry = 0;
        do
        {
            Pylon pylon = pylons.ElementAtOrDefault(_rng.Next(pylons.Count()));
            Vector2 randomCircle = UnityEngine.Random.insideUnitCircle * pylon.GetRange();
            Vector3 pos = new Vector3(pylon.transform.position.x + randomCircle.x, 0f, pylon.transform.position.z + randomCircle.y);
            if (-50f < pos.x && pos.x < 50f && -50f < pos.z && pos.z < 50f)
            {
                pylon.BuildMine(pos);
                AddReward(0.01f);
                retry = 10;
            }
            else
            {
                retry++;
            }
        }
        while (retry < 10);
    }
}
