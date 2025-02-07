using UnityEngine;

public class Factory : Building
{
    private AssetPool cubepool;

    new private void Start()
    {
        base.Start();
        // This is so fucked up man
        cubepool = GameObject
            .FindWithTag("CubePool")
            .GetComponent<AssetPool>();

    }

    // Update is called once per frame
    new private void Update()
    {
        Debug.Log(cubepool.CountAll());
        if (IsSelected && Input.GetMouseButtonDown(1))
        {
            GameObject newCube = cubepool.Get();
            newCube.transform.position = gameObject.transform.position;
            newCube.transform.rotation = gameObject.transform.rotation;
        }
    }
}
