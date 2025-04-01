using UnityEngine;

public class MasterController : MonoBehaviour
{
    public Asset cube;
    public Asset sphere;
    public Asset tetra;
    public Asset factory;

    private void Start()
    {
        Player player1 = GetComponent<TeamController>().GetPlayer(1);
        Player player2 = GetComponent<TeamController>().GetPlayer(2);
        Camera mainCam = Camera.main;
        mainCam.GetComponent<PlayerComponent>().player = player1;

        Asset newCube = Instantiate(cube);
        Asset newSphere = Instantiate(sphere);
        Asset newTetra = Instantiate(tetra);
        Asset newFactory = Instantiate(factory);

        newCube.GetComponent<PlayerComponent>().player = player1;
        newSphere.GetComponent<PlayerComponent>().player = player1;
        newTetra.GetComponent<PlayerComponent>().player = player2;
        newFactory.GetComponent<PlayerComponent>().player = player1;
        player1.AddPlayerAsset(newCube);
        player1.AddPlayerAsset(newSphere);
        player2.AddPlayerAsset(newTetra);
        player1.AddPlayerAsset(newFactory);
    }

    private void FixedUpdate()
    {
        
    }
}
