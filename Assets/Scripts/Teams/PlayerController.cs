using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum Command
{
    Move,
    AttackMove,
    Attack,
    Stop,
    None
}

public class PlayerController : MonoBehaviour
{
    private Vector2? _startPos;
    private Vector2? _endPos;
    private CommandbarBehaviour _commandbar;
    private InfobarBehaviour _infobar;
    private ResourcebarBehaviour _resourcebar;
    private Command _command = Command.None;

    private Player player { get; set; }
    private Camera mainCam { get; set; }
    private Mouse mouse { get; set; }
    public List<Asset> hovered { get; private set; } = new List<Asset>();
    public List<Asset> selected { get; private set; } = new List<Asset>();

    private void Start()
    {
        player = GetComponent<PlayerComponent>().player;
        player.setController(this);
        mainCam = Camera.main;
        mouse = Mouse.current;
        _commandbar = FindFirstObjectByType<CommandbarBehaviour>();
        _infobar = FindFirstObjectByType<InfobarBehaviour>();
        _resourcebar = FindFirstObjectByType<ResourcebarBehaviour>();
        _commandbar.gameObject.SetActive(false);
        _infobar.gameObject.SetActive(false);
    }

    private void Update()
    {
        SelectAssets();
        CommandAssets();
    }

    private void OnGUI()
    {
        if (_startPos != null && _endPos != null)
        {
            Rect selection = SomeRect(_startPos.Value, _endPos.Value);
            selection.yMin = Screen.height - selection.yMin;
            selection.yMax = Screen.height - selection.yMax;
            GUI.DrawTexture(selection, Texture2D.grayTexture);
        }
        if (selected.Count > 0)
        {
            _commandbar.gameObject.SetActive(true);
            _infobar.gameObject.SetActive(true);
            _infobar.setUnitName(selectedToString());
        }
        else
        {
            _commandbar.gameObject.SetActive(false);
            _infobar.gameObject.SetActive(false);
        }
        _resourcebar.setPolyCount(player.team.poly);
    }

    private void SelectAssets()
    {
        Vector2? pos = mouse.position.value;
        if (mouse.leftButton.wasReleasedThisFrame)
        {
            selected = new List<Asset>(hovered);
            selected.ForEach(a => a.GetComponent<Asset>().SetHalo(Asset.SELECT_INTENSITY));
            hovered.Clear();
            _startPos = _endPos = null;
        }
        else if (mouse.leftButton.wasPressedThisFrame)
        {
            selected.ForEach(a => a.GetComponent<Asset>().SetHalo(Asset.IDLE_INTENSITY));
            selected.Clear();
            _startPos = pos;
        }
        if (mouse.leftButton.isPressed)
        {
            _endPos = pos;
            Rect selection = SomeRect(_startPos.Value, _endPos.Value);
            foreach (Asset asset in player.assets)
            {
                Vector2 assetPos = mainCam.WorldToScreenPoint(asset.transform.position);
                if (selection.Contains(assetPos))
                {
                    if (!hovered.Contains(asset))
                    {
                        hovered.Add(asset);
                        asset.GetComponent<Asset>().SetHalo(Asset.HOVER_INTENSITY);
                    }
                }
                else if (hovered.Contains(asset))
                {
                    hovered.Remove(asset);
                    asset.GetComponent<Asset>().SetHalo(Asset.IDLE_INTENSITY);
                }
            }
        }
    }

    private void CommandAssets()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            SetCommand(Command.AttackMove);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            SetCommand(Command.Stop);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            SetCommand(Command.Move);
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            SetCommand(Command.Attack);
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetCommand(Command.None);
        }

        /*
        if (mouse.rightButton.wasPressedThisFrame && Input.GetKey(KeyCode.A))
        {
            Ray ray = mainCam.ScreenPointToRay(mouse.position.value);
            AttackMoveAssets(ray);
        }
        else if (mouse.rightButton.wasPressedThisFrame)
        {
            Ray ray = mainCam.ScreenPointToRay(mouse.position.value);
            MoveAssets(ray);
        }
        */
        BuildAssets();
        
    }

    public void SetCommand(Command command)
    {
        _command = command;
    }

    private void AttackAssets(Ray ray)
    {
        foreach (Asset asset in selected) if (asset.GetComponent<Unit>())
            {
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.GetComponent<Asset>())
                {
                    asset.GetComponent<Unit>().AttackCmd(hit.collider.gameObject.GetComponent<Asset>());
                }
            }
    }

    private void AttackMoveAssets(Ray ray)
    {
        foreach (Asset asset in selected) if (asset.GetComponent<Unit>())
        {
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                asset.GetComponent<Unit>().AttackMoveCmd(hit.point);
            }
        }
    }

    private void MoveAssets(Ray ray)
    {
        foreach (Asset asset in selected) if (asset.GetComponent<Unit>())
        {
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                asset.GetComponent<Unit>().MoveCmd(hit.point);
            }
        }
    }

    private void BuildAssets()
    {
        foreach (Asset asset in selected) if (asset.GetComponent<Factory>())
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                asset.GetComponent<Factory>().BuildCube();
            }
            else if (Input.GetKeyDown(KeyCode.V))
            {
                asset.GetComponent<Factory>().BuildSphere();
            }
            else if (Input.GetKeyDown(KeyCode.C))
            {
                asset.GetComponent<Factory>().BuildTetra();
            }
        }
    }

    public void RemoveAsset(Asset asset)
    {
        hovered.Remove(asset);
        selected.Remove(asset);
    }

    Rect SomeRect(Vector2 startPos, Vector2 endPos)
    {
        float xMin = Mathf.Min(startPos.x, endPos.x);
        float yMin = Mathf.Min(startPos.y, endPos.y);
        float xMax = Mathf.Max(startPos.x, endPos.x);
        float yMax = Mathf.Max(startPos.y, endPos.y);
        return Rect.MinMaxRect(xMin, yMin, xMax, yMax);
    }

    public string selectedToString()
    {
        int cubeCount = 0;
        int sphereCount = 0;
        int tetraCount = 0;
        int factoryCount = 0;
        int pylonCount = 0;
        int mineCount = 0;
        foreach (Asset asset in selected)
        {
            switch (asset)
            {
                case Cube:
                    {
                        cubeCount++;
                        break;
                    }
                case Sphere:
                    {
                        sphereCount++;
                        break;
                    }
                case Tetra:
                    {
                        tetraCount++;
                        break;
                    }
                case Factory:
                    {
                        factoryCount++;
                        break;
                    }
                case Pylon:
                    {
                        pylonCount++;
                        break;
                    }
                case Mine:
                    {
                        mineCount++;
                        break;
                    }
                default:
                    break;
            }
        }
        return ("Cube x" + cubeCount + ", Sphere x" + sphereCount + ", Tetra x" + tetraCount);
    }
}
