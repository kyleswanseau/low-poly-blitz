using UnityEngine;

public class Player
{
    private static int _nextIndex = 0;

    public Team? team { get; private set; } = null;
    public Material material { get; private set; }
    public int index { get; private set; }

    public Player()
    {
        index = _nextIndex;
        _nextIndex++;
    }

    public Player(Material material) : this()
    {
        this.material = material;
    }

    public void SetTeam(Team team)
    {
        if (this.team != team)
        {
            this.team = team;
            team.AddPlayer(this);
        }
    }
}
