using UnityEngine;

public class PlayerComponent : MonoBehaviour
{
    private Player? _player = null;

    public Player? GetPlayer()
    {
        return _player;
    }

    public void SetPlayer(Player player)
    {
        this._player = player;
        if (GetComponent<Renderer>())
        {
            GetComponent<Renderer>().material = player.material;
        }
    }

    public void ClearPlayer()
    {
        this._player = null;
    }
}
