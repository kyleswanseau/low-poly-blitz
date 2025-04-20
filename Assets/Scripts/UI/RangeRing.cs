using UnityEngine;

public class RangeRing : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    private int _lineCount = 100;
    private float _radius = 0f;
    private float _width = 0.2f;

    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.loop = true;
        _radius = transform.parent.GetComponent<Asset>().GetRange();
        Draw();
        SetVisible(false);
    }

    private void Draw()
    {
        _lineRenderer.positionCount = _lineCount;
        _lineRenderer.startWidth = _width;
        float theta = (2f * Mathf.PI) / _lineCount;
        float angle = 0f;
        for (int i = 0; i < _lineCount; i++, angle += theta)
        {
            float x = _radius * Mathf.Cos(angle);
            float z = _radius * Mathf.Sin(angle);
            _lineRenderer.SetPosition(i, new Vector3(x, 0, z));
        }
    }

    public void SetVisible(bool set)
    {
        if (null != _lineRenderer)
        {
            _lineRenderer.enabled = set;
        }
    }
}