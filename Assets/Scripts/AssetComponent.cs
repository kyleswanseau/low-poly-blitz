using UnityEngine;

public class AssetComponent : MonoBehaviour
{
    private Light haloComp;
    private static Camera mainCam;
    private bool isHovered = false;
    private bool isSelected = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCam = Camera.main;
        Light? halo = gameObject.GetComponentInChildren<Light>();
        if (halo != null)
        {
            halo.intensity = 100;
            halo.enabled = isSelected;
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

    public Vector3 getPositionInCam()
    {
        return mainCam.WorldToScreenPoint(gameObject.transform.position);
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
