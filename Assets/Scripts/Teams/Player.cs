using UnityEngine;

public class Player
{
    private static int _nextIndex = 0;
    private Team _team = Team.neutralTeam;

    public static readonly Player neutralPlayer = new Player(Team.neutralTeam);

    public Team team
    {
        get { return _team; }
        set
        {
            _team.RemovePlayer(this);
            _team = value;
            material = team.material;
            _team.AddPlayer(this);
        }
    }
    public Material material { get; private set; }
    public int index { get; private set; }

    public Player()
    {
        index = _nextIndex;
        _nextIndex++;
    }

    public Player(Team team) : this()
    {
        this.team = team;
    }
}
