using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Vector2? _startPos;
    private Vector2? _endPos;

    private Player player { get; set; }
    private Camera mainCam { get; set; }
    private Mouse mouse { get; set; }
    public List<GameObject> hovered { get; private set; } = new List<GameObject>();
    public List<GameObject> selected { get; private set; } = new List<GameObject>();

    private void Start()
    {
        player = GetComponent<PlayerComponent>().player;
        mainCam = Camera.main;
        mouse = Mouse.current;
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
    }

    private void SelectAssets()
    {
        Vector2? pos = mouse.position.value;
        if (mouse.leftButton.wasReleasedThisFrame)
        {
            selected = new List<GameObject>(hovered);
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
            foreach (GameObject asset in player.assets)
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
        if (mouse.rightButton.wasPressedThisFrame)
        {
            Ray ray = mainCam.ScreenPointToRay(mouse.position.value);
            MoveAssets(ray);
        }
        BuildAssets();
    }

    private void MoveAssets(Ray ray)
    {
        foreach (GameObject asset in selected) if (asset.GetComponent<Unit>())
        {
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                asset.GetComponent<Unit>().MoveTo(hit.point);
            }
        }
    }

    private void BuildAssets()
    {
        foreach (GameObject asset in selected) if (asset.GetComponent<Factory>())
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

    Rect SomeRect(Vector2 startPos, Vector2 endPos)
    {
        float xMin = Mathf.Min(startPos.x, endPos.x);
        float yMin = Mathf.Min(startPos.y, endPos.y);
        float xMax = Mathf.Max(startPos.x, endPos.x);
        float yMax = Mathf.Max(startPos.y, endPos.y);
        return Rect.MinMaxRect(xMin, yMin, xMax, yMax);
    }
}
