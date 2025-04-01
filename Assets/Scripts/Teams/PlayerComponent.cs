using UnityEngine;

public class PlayerComponent : MonoBehaviour
{
    private Player _player;
    private static PlayerController _controller;

    public Player player
    {
        get { return _player; }
        set
        {
            _player = value;
            if (GetComponent<Renderer>())
            {
                GetComponent<Renderer>().material = _player.material;
            }
        }
    }

    private void Awake()
    {
        _player = Player.neutralPlayer;
    }
}
