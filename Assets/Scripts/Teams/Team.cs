using System.Collections.Generic;
using UnityEngine;

public class Team
{
    private static Dictionary<int, Player> _players = new Dictionary<int, Player>();
    private static int _nextIndex = 0;

    public Material material { get; private set; }
    public int index { get; private set; }

    public Team()
    {
        index = _nextIndex;
        _nextIndex++;
    }

    public Team(Material material) : this()
    {
        this.material = material;
    }

    public void AddPlayer(Player player)
    {
        if (!_players.ContainsKey(player.index))
        {
            _players.Add(player.index, player);
            player.SetTeam(this);
        }
    }

    public void RemovePlayer(Player player)
    {
        _players.Remove(player.index);
        player.SetTeam(null);
    }

    public Dictionary<int, Player> GetPlayers()
    {
        return _players;
    }
}
