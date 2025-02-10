using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Vector2? _startPos;
    private Vector2? _endPos;

    private Mouse mouse { get; set; }
    private List<GameObject> assets { get; set; } = new List<GameObject>();

    private void Start()
    {
        mouse = Mouse.current;
    }

    private void Update()
    {
        SelectAssets();
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
            _endPos = pos;
            Rect selection = SomeRect(_startPos.Value, _endPos.Value);
            foreach (GameObject asset in assets)
            {
                Vector2 assetPos = asset.GetComponent<Asset>().getPositionInCam();
                asset.GetComponent<Asset>().setSelected(selection.Contains(assetPos));
            }
            _startPos = _endPos = null;
        }
        else if (mouse.leftButton.wasPressedThisFrame)
        {
            foreach (GameObject asset in assets)
            {
                asset.GetComponent<Asset>().setSelected(false);
            }
            _startPos = pos;
        }
        if (mouse.leftButton.isPressed)
        {
            _endPos = pos;
            Rect selection = SomeRect(_startPos.Value, _endPos.Value);
            foreach (GameObject asset in assets)
            {
                Vector2 assetPos = asset.GetComponent<Asset>().getPositionInCam();
                asset.GetComponent<Asset>().setHovered(selection.Contains(assetPos));
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

    public void AddPlayerAsset(GameObject asset)
    {
        if (null != asset.GetComponent<PlayerComponent>())
        {
            Player player = gameObject.GetComponent<PlayerComponent>().GetPlayer();
            asset.GetComponent<PlayerComponent>().SetPlayer(player);
            assets.Add(asset);
        }
        else
        {
            Debug.LogError("Attempted to add invalid game object to player control.");
        }
    }

    public void RemovePlayerAsset(GameObject asset)
    {
        asset.GetComponent<PlayerComponent>().ClearPlayer();
        assets.Remove(asset);
    }

    public List<GameObject> GetPlayerAssets()
    {
        return assets;
    }
}
