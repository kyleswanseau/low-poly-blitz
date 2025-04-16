using UnityEngine;
using UnityEngine.UI;

public class CommandbarBehaviour : MonoBehaviour
{
    [SerializeField] private Button _moveButton;
    [SerializeField] private Button _attackMoveButton;
    [SerializeField] private Button _stopButton;
    [SerializeField] private Button _attackButton;
    [SerializeField] private Button _rallyButton;
    [SerializeField] private Button _facstopButton;
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
        SwitchGUI(0);
    }

    public void SwitchGUI(int mode)
    {
        switch (mode)
        {
            case 1:
                _moveButton.gameObject.SetActive(true);
                _attackMoveButton.gameObject.SetActive(true);
                _stopButton.gameObject.SetActive(true);
                _attackButton.gameObject.SetActive(true);
                _rallyButton.gameObject.SetActive(false);
                _facstopButton.gameObject.SetActive(false);
                break;
            case 2:
            case 3:
                _moveButton.gameObject.SetActive(false);
                _attackMoveButton.gameObject.SetActive(false);
                _stopButton.gameObject.SetActive(false);
                _attackButton.gameObject.SetActive(false);
                _rallyButton.gameObject.SetActive(false);
                _facstopButton.gameObject.SetActive(false);
                break;
            default:
                _moveButton.gameObject.SetActive(false);
                _attackMoveButton.gameObject.SetActive(false);
                _stopButton.gameObject.SetActive(false);
                _attackButton.gameObject.SetActive(false);
                _rallyButton.gameObject.SetActive(true);
                _facstopButton.gameObject.SetActive(true);
                break;
        }
    }

    public void Move()
    {
        _controller.SetCommand(ECommand.Move);
    }

    public void AttackMove()
    {
        _controller.SetCommand(ECommand.AttackMove);
    }

    public void Stop()
    {
        _controller.SetCommand(ECommand.Stop);
    }

    public void Attack()
    {
        _controller.SetCommand(ECommand.Attack);
    }
}
