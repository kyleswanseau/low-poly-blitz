using System.Collections.Generic;
using UnityEngine;

public class Player
{
    private static int _nextIndex = 0;
    private Team _team = Team.neutralTeam;

    public static readonly Player neutralPlayer = new Player(Team.neutralTeam);
    public List<GameObject> assets { get; private set; } = new List<GameObject>();

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

    public void AddPlayerAsset(GameObject asset)
    {
        if (asset.GetComponent<PlayerComponent>())
        {
            asset.GetComponent<PlayerComponent>().player = this;
            assets.Add(asset);
        }
        else
        {
            Debug.LogError("Attempted to add invalid game object to player control.");
        }
    }

    public void RemovePlayerAsset(GameObject asset)
    {
        if (asset.GetComponent<PlayerComponent>())
        {
            asset.GetComponent<PlayerComponent>().player = neutralPlayer;
            assets.Remove(asset);
        }
        else
        {
            Debug.LogError("Attempted to add invalid game object to player control.");
        }
    }
}
