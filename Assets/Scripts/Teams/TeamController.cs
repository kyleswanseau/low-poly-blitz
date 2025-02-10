using System.Collections.Generic;
using UnityEngine;

public class TeamController : MonoBehaviour
{
    private Dictionary<int, Team> _teamDict = new Dictionary<int, Team>();
    private Dictionary<int, Player>_playerDict = new Dictionary<int, Player>();

    private void Awake()
    {
        Material red = Resources.Load<Material>("Materials/Red");
        Material green = Resources.Load<Material>("Materials/Green");
        Material blue = Resources.Load<Material>("Materials/Blue");
        Team team0 = new Team(blue);
        Team team1 = new Team(red);
        Player player0 = new Player(green);
        Player player1 = new Player(red);

        player0.SetTeam(team0);
        player1.SetTeam(team1);
        _teamDict.Add(team0.index, team0);
        _teamDict.Add(team1.index, team1);
        _playerDict.Add(player0.index, player0);
        _playerDict.Add(player1.index, player1);
    }

    public Dictionary<int, Team> GetTeams()
    {
        return _teamDict;
    }

    public Team GetTeam(int index)
    {
        return _teamDict[index];
    }

    public Dictionary<int, Player> GetPlayers()
    {
        return _playerDict;
    }

    public Player GetPlayer(int index)
    {
        return _playerDict[index];
    }
}
