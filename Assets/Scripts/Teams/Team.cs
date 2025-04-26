using System.Collections.Generic;
using UnityEngine;

public class Team
{
    private Dictionary<int, Player> _players = new Dictionary<int, Player>();
    private static int _nextIndex = 0;

    public static readonly Team neutralTeam = new Team(Resources.Load<Material>("Materials/Gray"));
    public Material material { get; private set; }
    public int index { get; private set; }
    public float poly { get; private set; } = 0f;
    public float income { get; private set; } = 0f;
    public float expense { get; private set; } = 0f;

    private Team()
    {
        index = _nextIndex;
        _nextIndex++;
    }

    public Team(Material material) : this()
    {
        this.material = material;
    }

    public void Update()
    {
        poly += income;
    }

    public void AddPlayer(Player player)
    {
        if (!_players.ContainsKey(player.index))
        {
            _players.Add(player.index, player);
            if (this != player.team)
            {
                player.team = this;
            }
        }
    }

    public void RemovePlayer(Player player)
    {
        if (_players.ContainsKey(player.index))
        {
            _players.Remove(player.index);
            if (Team.neutralTeam != player.team)
            {
                player.team = Team.neutralTeam;
            }
        }
    }

    public Dictionary<int, Player> GetPlayers()
    {
        return _players;
    }

    public void AddPoly(float poly)
    {
        this.poly += poly;
    }

    public void AddIncome(float income)
    {
        this.income += income;
    }

    public void AddExpense(float expense)
    {
        this.expense += expense;
    }
}
