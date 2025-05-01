using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public enum ECommand
{
    Move,
    AttackMove,
    Attack,
    Stop,
    None
}

public enum EBuild
{
    Factory,
    Pylon,
    Mine,
    None
}

public class PlayerController : MonoBehaviour
{
    [SerializeField] private int _playerIndex;
    private Vector2? _startPos;
    private Vector2? _endPos;
    private CommandbarBehaviour _commandbar;
    private InfobarBehaviour _infobar;
    private ResourcebarBehaviour _resourcebar;
    private ECommand _command = ECommand.None;
    private EBuild _buildBuilding = EBuild.None;

    private Player player { get; set; }
    private Camera mainCam { get; set; }
    private Mouse mouse { get; set; }
    public Asset? hoveredSingle { get; private set; } = null;
    public Asset? selectedSingle { get; private set; } = null;
    public List<Asset> hoveredMultiple { get; private set; } = new List<Asset>();
    public List<Asset> selected { get; private set; } = new List<Asset>();

    private void Start()
    {
        TeamController tc = FindFirstObjectByType<TeamController>();
        player = tc.GetPlayer(_playerIndex);
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
        if (
            (_commandbar.isActiveAndEnabled && _commandbar.rect.Contains(mouse.position.value)) ||
            (_infobar.isActiveAndEnabled && _infobar.rect.Contains(mouse.position.value)) ||
            (_resourcebar.isActiveAndEnabled && _resourcebar.rect.Contains(mouse.position.value))
        )
        {
            
        }
        else
        {
            SelectAssets();
        }
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
            foreach (Asset asset in selected)
            {
                if (asset is Factory factory)
                {
                    _infobar.setFactory(factory);
                }
            }
        }
        else
        {
            _commandbar.gameObject.SetActive(false);
            _infobar.gameObject.SetActive(false);
        }
        _resourcebar.setPolyCount(player.team.poly, player.team.income, player.team.expense);
    }

    private void SelectAssets()
    {
        Vector2? pos = mouse.position.value;
        Ray ray = mainCam.ScreenPointToRay(mouse.position.value);
        RaycastHit hit;
        if (mouse.leftButton.wasReleasedThisFrame)
        {
            if (null != hoveredSingle && hoveredMultiple.Count <= 0)
            {
                // Click selection
                if (Physics.Raycast(ray, out hit) &&
                    hit.collider.gameObject.GetComponent<Asset>() &&
                    hit.collider.gameObject.GetComponent<Asset>() == hoveredSingle &&
                    hit.collider.gameObject.GetComponent<PlayerComponent>().player == player)
                {
                    selected.Clear();
                    selected.Add(hoveredSingle);
                    hoveredSingle = null;
                }
            }
            else
            {
                // Drag selection
                selected = new List<Asset>(hoveredMultiple);
                hoveredMultiple.Clear();
            }
            selected.ForEach(s => s.SetHalo(EIntensity.Select));
            selected.ForEach(s => s.SetRange(true));
            if (selected.Any(asset => asset is Factory))
            {
                _commandbar.SwitchGUI(1);
                _infobar.SwitchGUI(1);
            }
            else if (selected.Any(asset => asset is Pylon))
            {
                _commandbar.SwitchGUI(2);
                _infobar.SwitchGUI(2);
            }
            else if (selected.Any(asset => asset is Mine))
            {
                _commandbar.SwitchGUI(3);
                _infobar.SwitchGUI(3);
            }
            else
            {
                _commandbar.SwitchGUI(0);
                _infobar.SwitchGUI(0);
            }
            _startPos = _endPos = null;
        }
        else if (mouse.leftButton.wasPressedThisFrame)
        {
            if (null != hoveredSingle)
            {
                // Click selection
            }
            else if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.GetComponent<Asset>())
            {
                // Drag selection
                hoveredMultiple.Add(hit.collider.gameObject.GetComponent<Asset>());
            }
            selected.ForEach(s => s.SetHalo(EIntensity.Idle));
            selected.ForEach(s => s.SetRange(false));
            selected.Clear();
            _startPos = pos;
        }
        if (mouse.leftButton.isPressed)
        {
            _endPos = pos;
            Rect selection = SomeRect(_startPos.Value, _endPos.Value);
            foreach (Asset asset in player.assets)
            {
                if (asset is Unit)
                {
                    Vector2 assetPos = mainCam.WorldToScreenPoint(asset.transform.position);
                    if (selection.Contains(assetPos))
                    {
                        if (!hoveredMultiple.Contains(asset))
                        {
                            hoveredMultiple.Add(asset);
                            asset.SetHalo(EIntensity.Hover);
                        }
                    }
                    else if (hoveredMultiple.Contains(asset))
                    {
                        hoveredMultiple.Remove(asset);
                        asset.SetHalo(EIntensity.Idle);
                    }
                }
            }
        }
        else if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.GetComponent<Asset>())
        {
            if (null != hoveredSingle && !selected.Contains(hoveredSingle))
            {
                hoveredSingle.SetHalo(EIntensity.Idle);
            }
            hoveredSingle = hit.collider.gameObject.GetComponent<Asset>();
            if (!selected.Contains(hoveredSingle))
            {
                hoveredSingle.SetHalo(EIntensity.Hover);
            }
        }
        else if (null != hoveredSingle)
        {
            if (!selected.Contains(hoveredSingle))
            {
                hoveredSingle.SetHalo(EIntensity.Idle);
            }
            hoveredSingle = null;
        }
    }

    private void CommandAssets()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            SetCommand(ECommand.AttackMove);
        }
        else if (Input.GetKeyDown(KeyCode.S) || _command == ECommand.Stop)
        {
            StopAssets();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            SetCommand(ECommand.Move);
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            SetCommand(ECommand.Attack);
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetCommand(ECommand.None);
        }
        if (mouse.rightButton.wasPressedThisFrame)
        {
            Ray ray = mainCam.ScreenPointToRay(mouse.position.value);
            if (selected.Any(asset => asset is Pylon))
            {
                RaycastHit hit;
                Physics.Raycast(ray, out hit);
                foreach (Pylon pylon in selected)
                {
                    if (hit.collider.gameObject.layer == 6 &&
                        Vector3.Distance(pylon.transform.position, hit.point) <= Pylon.RANGE)
                    {
                        switch (_buildBuilding)
                        {
                            case EBuild.Factory:
                                pylon.BuildFactory(hit.point);
                                _buildBuilding = EBuild.None;
                                break;
                            case EBuild.Pylon:
                                pylon.BuildPylon(hit.point);
                                _buildBuilding = EBuild.None;
                                break;
                            case EBuild.Mine:
                                pylon.BuildMine(hit.point);
                                _buildBuilding = EBuild.None;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            else
            {
                switch (_command)
                {
                    case ECommand.Move:
                        MoveAssets(ray);
                        break;
                    case ECommand.AttackMove:
                        AttackMoveAssets(ray);
                        break;
                    case ECommand.Attack:
                        AttackAssets(ray);
                        break;
                    case ECommand.Stop:
                        StopAssets();
                        break;
                    default:
                        RaycastHit hit;
                        if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.GetComponent<Asset>())
                        {
                            AttackAssets(ray);
                        }
                        else if (Physics.Raycast(ray, out hit))
                        {
                            MoveAssets(ray);
                        }
                        break;
                }
            }    
        }
        BuildAssets();
    }

    private void BuildAssets()
    {
        foreach (Asset asset in selected)
        {
            if (asset is Factory factory)
            {
                if (Input.GetKeyDown(KeyCode.C))
                {
                    factory.BuildCubes();
                }
                else if (Input.GetKeyDown(KeyCode.V))
                {
                    factory.BuildSpheres();
                }
                else if (Input.GetKeyDown(KeyCode.B))
                {
                    factory.BuildTetras();
                }
            }
            if (asset is Pylon pylon)
            {
                if (Input.GetKeyDown(KeyCode.C))
                {
                    _buildBuilding = EBuild.Factory;
                }
                else if (Input.GetKeyDown(KeyCode.V))
                {
                    _buildBuilding = EBuild.Pylon;
                }
                else if (Input.GetKeyDown(KeyCode.B))
                {
                    _buildBuilding = EBuild.Mine;
                }
                else if (Input.GetKeyDown(KeyCode.Escape))
                {
                    _buildBuilding = EBuild.None;
                }
            }
        }
    }

    public void SetCommand(ECommand command)
    {
        _command = command;
    }

    private void MoveAssets(Ray ray)
    {
        foreach (Asset asset in selected)
        {
            if (asset.GetComponent<Unit>())
            {
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    asset.GetComponent<Unit>().MoveCmd(hit.point);
                }
            }
            if (asset is Factory factory)
            {
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    factory.GetComponent<Factory>().MoveCmd(hit.point);
                }
            }
        }
    }

    private void AttackMoveAssets(Ray ray)
    {
        foreach (Asset asset in selected)
        {
            if (asset.GetComponent<Unit>())
            {
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    asset.GetComponent<Unit>().AttackMoveCmd(hit.point);
                }
            }
        }
    }

    private void AttackAssets(Ray ray)
    {
        foreach (Asset asset in selected)
        {
            if (asset.GetComponent<Unit>())
            {
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.GetComponent<Asset>())
                {
                    asset.GetComponent<Unit>().AttackCmd(hit.collider.gameObject.GetComponent<Asset>());
                }
            }
        }
    }

    private void StopAssets()
    {
        foreach (Asset asset in selected)
        {
            if (asset.GetComponent<Unit>())
            {
                asset.GetComponent<Unit>().StopCmd();
            }
            if (asset is Factory factory)
            {
                factory.GetComponent<Factory>().StopCmd();
            }
        }
    }

    public void RemoveAsset(Asset asset)
    {
        hoveredMultiple.Remove(asset);
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
        string output = "";
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
        if (cubeCount > 0)
        {
            output += "Cube x" + cubeCount + " ";
        }
        if (sphereCount > 0)
        {
            output += "Sphere x" + sphereCount + " ";
        }
        if (tetraCount > 0)
        {
            output += "Tetra x" + tetraCount + " ";
        }
        if (factoryCount > 0)
        {
            output += "Factory x" + factoryCount + " ";
        }
        if (pylonCount > 0)
        {
            output += "Pylon x" + pylonCount + " ";
        }
        if (mineCount > 0)
        {
            output += "Mine x" + mineCount + " ";
        }
        return output;
    }
}
