using UnityEngine;

public abstract class Asset : MonoBehaviour
{
    protected Camera _mainCam;
    
    protected abstract AssetPool pool { get; set; }
    protected abstract int health { get; set; }
    public bool isHovered { get; set; } = false;
    public bool isSelected { get; set; } = false;

    protected virtual void Start()
    {
        _mainCam = Camera.main;
        Light halo = gameObject.GetComponentInChildren<Light>();
        halo.intensity = 100;
        halo.enabled = isSelected;
    }

    protected virtual void FixedUpdate()
    {
        //CheckHealth();
    }

    protected virtual void Damage(int damage)
    {
        health -= damage;
        CheckHealth();
    }

    protected abstract void CheckHealth();

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
