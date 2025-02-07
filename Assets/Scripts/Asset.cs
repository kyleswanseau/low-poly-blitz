using UnityEngine;

public class Asset : MonoBehaviour
{
    //private Light haloComp;
    protected static Camera mainCam;
    public bool IsHovered { get; set; } = false;
    public bool IsSelected { get; set; } = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected void Start()
    {
        mainCam = Camera.main;
        Light? halo = gameObject.GetComponentInChildren<Light>();
        if (halo != null)
        {
            halo.intensity = 100;
            halo.enabled = IsSelected;
        }
        /*
        GameObject haloObj = new GameObject("Halo");
        haloComp = haloObj.AddComponent<Light>();
        haloObj.transform.SetParent(gameObject.transform);
        haloObj.transform.SetPositionAndRotation(new Vector3(0, 5, 0), Quaternion.Euler(90, 0, 0));

        haloComp.enabled = isSelected;
        haloComp.type = LightType.Spot;
        haloComp.color = Color.green;
        haloComp.intensity = 100;
        haloComp.innerSpotAngle = 20;
        haloComp.spotAngle = 25;
        */
    }

    protected void Update()
    {

    }

    public Vector3 getPositionInCam()
    {
        return mainCam.WorldToScreenPoint(gameObject.transform.position);
    }

    public void setHovered(bool hovered)
    {

        IsHovered = hovered;
        Light? halo = gameObject.GetComponentInChildren<Light>();
        if (halo != null)
        {
            halo.intensity = 20;
            halo.enabled = IsHovered;
        }
    }

    public void setSelected(bool selected)
    {
        IsSelected = selected;
        if (IsSelected)
        {
            setHovered(false);
        }
        Light? halo = gameObject.GetComponentInChildren<Light>();
        if (halo != null)
        {
            halo.intensity = 100;
            halo.enabled = IsSelected;
        }
    }
}
