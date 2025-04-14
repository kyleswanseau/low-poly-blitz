using UnityEngine;
using UnityEngine.UI;

public class CommandbarBehaviour : MonoBehaviour
{
    [SerializeField] private Button _moveButton;
    [SerializeField] private Button _attackMoveButton;
    [SerializeField] private Button _stopButton;
    [SerializeField] private Button _attackButton;
    private PlayerController _controller;

    public Rect rect { get; private set; }

    private void Start()
    {
        _controller = Camera.main.GetComponent<PlayerController>();
        _moveButton.onClick.AddListener(Move);
        _attackMoveButton.onClick.AddListener(AttackMove);
        _stopButton.onClick.AddListener(Stop);
        _attackButton.onClick.AddListener(Attack);
        RectTransform rt = GetComponent<RectTransform>();
        Vector2 min = Vector2.Scale(rt.anchorMin, new Vector2(Screen.width, Screen.height));
        Vector2 max = Vector2.Scale(rt.anchorMax, new Vector2(Screen.width, Screen.height));
        rect = Rect.MinMaxRect(min.x, min.y, max.x, max.y);
    }

    private void Move()
    {
        _controller.SetCommand(Command.Move);
    }

    private void AttackMove()
    {
        _controller.SetCommand(Command.AttackMove);
    }

    private void Stop()
    {
        _controller.SetCommand(Command.Stop);
    }

    private void Attack()
    {
        _controller.SetCommand(Command.Attack);
    }
}
