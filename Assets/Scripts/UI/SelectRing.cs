using Unity.Mathematics;
using UnityEngine;

public enum EIntensity
{
    Idle,
    Hover,
    Select
}

public class SelectRing : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    private int _lineCount = 100;
    private float _radius = 1.5f;
    private float _width = 0f;

    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.loop = true;
        Draw();
        SetIntensity(EIntensity.Idle);
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

    public void SetIntensity(EIntensity intensity)
    {
        if (null != _lineRenderer)
        {
            switch (intensity)
            {
                case EIntensity.Idle:
                    _width = 0f;
                    _lineRenderer.enabled = false;
                    break;
                case EIntensity.Hover:
                    _width = 0.1f;
                    _lineRenderer.enabled = true;
                    break;
                case EIntensity.Select:
                    _width = 0.3f;
                    _lineRenderer.enabled = true;
                    break;
            }
            Draw();
        }
    }
}