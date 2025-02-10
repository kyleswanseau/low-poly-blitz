using UnityEngine;

public class Factory : Building
{
    private AssetPool _cubePool;
    private AssetPool _spherePool;
    private AssetPool _tetraPool;

    new private void Start()
    {
        base.Start();
        _cubePool = GameObject.FindWithTag("CubePool").GetComponent<AssetPool>();
        _spherePool = GameObject.FindWithTag("SpherePool").GetComponent<AssetPool>();
        _tetraPool = GameObject.FindWithTag("TetraPool").GetComponent<AssetPool>();
    }

    new private void Update()
    {
        if (isSelected && Input.GetKeyDown(KeyCode.B))
        {
            BuildCube();
        }
    }

    private void BuildCube()
    {
        Player owner = gameObject.GetComponent<PlayerComponent>().GetPlayer();
        GameObject newCube = _cubePool.Get();
        newCube.transform.position = gameObject.transform.position;
        newCube.transform.rotation = gameObject.transform.rotation;
        newCube.GetComponent<PlayerComponent>().SetPlayer(owner);
        _mainCam.GetComponent<PlayerController>().AddPlayerAsset(newCube);
    }

    private void BuildSphere()
    {

    }

    private void BuildTetra()
    {

    }
}
