using UnityEngine;
using UnityEngine.UI;

public class CommandbarBehaviour : MonoBehaviour
{
    [SerializeField] private Button _moveButton;
    [SerializeField] private Button _attackMoveButton;
    [SerializeField] private Button _stopButton;
    [SerializeField] private Button _attackButton;
    private PlayerController _controller;

    private void Start()
    {
        _controller = Camera.main.GetComponent<PlayerController>();
        _moveButton.onClick.AddListener(Move);
        _attackMoveButton.onClick.AddListener(AttackMove);
        _stopButton.onClick.AddListener(Stop);
        _attackButton.onClick.AddListener(Attack);
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
