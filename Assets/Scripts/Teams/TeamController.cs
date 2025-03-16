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
        Team team0 = Team.neutralTeam;
        Team team1 = new Team(blue);
        Team team2 = new Team(red);
        Player player0 = Player.neutralPlayer;
        Player player1 = new Player(team1);
        Player player2 = new Player(team2);

        _teamDict.Add(team0.index, team0);
        _teamDict.Add(team1.index, team1);
        _teamDict.Add(team2.index, team2);
        _playerDict.Add(player0.index, player0);
        _playerDict.Add(player1.index, player1);
        _playerDict.Add(player2.index, player2);
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
