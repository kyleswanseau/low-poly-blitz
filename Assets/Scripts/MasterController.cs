using UnityEngine;

public class MasterController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject cube = new GameObject("New Cube");
        cube.AddComponent<AssetComponent>();
        Instantiate(cube, new Vector3(5, 0.5f, 5), new Quaternion());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
