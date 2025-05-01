using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System.Linq;
using System.Collections.Generic;

public class UnitAI : Agent
{
    [SerializeField] private int allyID;
    [SerializeField] private int enemyID;
    [SerializeField] private MasterController _masterController;
    [SerializeField] private TeamController _teamController;
    private Team _team;
    private Player _player;
    private Player _enemy;
    private Queue<int> _alliedCountHistory = new Queue<int>();
    private Queue<int> _enemyCountHistory = new Queue<int>();
    private float persistentReward;

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
        _alliedCountHistory.Clear();
        _enemyCountHistory.Clear();
        persistentReward = 0f;
        if (Time.time > 0f)
        {
            _masterController.RestartUnitTraining();
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        int[,] grid = new int[10, 10];
        foreach (Asset asset in _enemy.assets)
        {
            Vector3 v3 = asset.transform.position + new Vector3(50f, 0f, 50f);
            int[] v2 = { Mathf.FloorToInt(v3.x / 10f), Mathf.FloorToInt(v3.z / 10f) };
            grid[v2[0], v2[1]]++;
        }

        IEnumerable<Asset> alliedUnits = _player.assets.Where(a => a is Unit);
        IEnumerable<Asset> enemyUnits = _enemy.assets.Where(a => a is Unit);
        sensor.AddObservation(alliedUnits.Count());
        sensor.AddObservation(enemyUnits.Count());
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                sensor.AddObservation(grid[i, j]);
            }
        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        if (actions.DiscreteActions[0] >= 1)
        {
            Vector2 v2 = new Vector2(actions.DiscreteActions[1] - 4.5f, actions.DiscreteActions[2] - 4.5f);
            Vector3 v3 = new Vector3(v2.x * 10f, 0f, v2.y * 10f);
            IEnumerable<Unit> alliedUnits = _player.assets.Where(a => a is Unit).Cast<Unit>();
            foreach (Unit unit in alliedUnits)
            {
                unit.AttackMoveCmd(v3);
            }
        }
    }

    public void ApplyReward()
    {
        float reward = 0f;
        IEnumerable<Asset> alliedUnits = _player.assets.Where(a => a is Unit);
        IEnumerable<Asset> enemyUnits = _enemy.assets.Where(a => a is Unit);
        int alliedCount = alliedUnits.Count();
        int enemyCount = enemyUnits.Count();

        if (_alliedCountHistory.Count > 2 && _enemyCountHistory.Count > 2)
        {
            if (_alliedCountHistory.Max() > alliedCount)
            {
                reward -= 0.01f * (_alliedCountHistory.Max() - alliedCount);
            }
            if (_enemyCountHistory.Max() > enemyCount)
            {
                reward += 0.1f * (_enemyCountHistory.Max() - enemyCount);
            }
            persistentReward += reward - 0.002f;
            if (persistentReward > 1f)
            {
                persistentReward = 1f;
            }
            else if (persistentReward < -1f)
            {
                persistentReward = -1f;
            }
            SetReward(persistentReward);
            if (persistentReward >= 1f)
            {
                Debug.Log("UnitAI: Passed");
                EndEpisode();
            }
            else if (persistentReward <= -1f)
            {
                Debug.Log("UnitAI: Failed");
                EndEpisode();
            }

            _alliedCountHistory.Enqueue(alliedCount);
            _enemyCountHistory.Enqueue(enemyCount);
            _alliedCountHistory.Dequeue();
            _enemyCountHistory.Dequeue();
        }
        else
        {
            _alliedCountHistory.Enqueue(alliedCount);
            _enemyCountHistory.Enqueue(enemyCount);
        }
    }
}
