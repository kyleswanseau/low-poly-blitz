using UnityEngine;

public class UnitComponent : AssetComponent
{
    public Camera cam;
    Vector3 pos;
    bool selected;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject cube = this.gameObject;
        Renderer renderer = cube.GetComponent<Renderer>();
        renderer.material.SetColor("_BaseColor", Color.black);
        selected = false;
    }

    // Update is called once per frame
    void Update()
    {
        pos = cam.WorldToScreenPoint(this.gameObject.transform.position);
    }

    public Vector3 getPos()
    {
        return pos;
    }

    public void setSelected(bool selected)
    {
        this.selected = selected;
        Renderer renderer = this.gameObject.GetComponent<Renderer>();
        if (this.selected)
        {
            renderer.material.SetColor("_BaseColor", Color.black);
        }
        else
        {
            renderer.material.SetColor("_BaseColor", Color.white);
        }
    }
}
