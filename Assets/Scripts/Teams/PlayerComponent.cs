using UnityEngine;

public class PlayerComponent : MonoBehaviour
{
    private Player _player;

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
