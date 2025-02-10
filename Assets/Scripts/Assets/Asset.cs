using UnityEngine;

public class Asset : MonoBehaviour
{
    protected static Camera _mainCam;

    public bool isHovered { get; set; } = false;
    public bool isSelected { get; set; } = false;

    protected void Start()
    {
        _mainCam = Camera.main;
        Light? halo = gameObject.GetComponentInChildren<Light>();
        if (halo != null)
        {
            halo.intensity = 100;
            halo.enabled = isSelected;
        }
    }

    protected void Update()
    {

    }

    public Vector3 getPositionInCam()
    {
        return _mainCam.WorldToScreenPoint(gameObject.transform.position);
    }

    public void setHovered(bool hovered)
    {

        isHovered = hovered;
        Light? halo = gameObject.GetComponentInChildren<Light>();
        if (halo != null)
        {
            halo.intensity = 20;
            halo.enabled = isHovered;
        }
    }

    public void setSelected(bool selected)
    {
        isSelected = selected;
        if (isSelected)
        {
            setHovered(false);
        }
        Light? halo = gameObject.GetComponentInChildren<Light>();
        if (halo != null)
        {
            halo.intensity = 100;
            halo.enabled = isSelected;
        }
    }
}
