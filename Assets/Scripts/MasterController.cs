using UnityEngine;

public class MasterController : MonoBehaviour
{
    public GameObject cube;
    public GameObject sphere;
    public GameObject tetra;
    public GameObject factory;

    private void Start()
    {
        Player player1 = GetComponent<TeamController>().GetPlayer(1);
        Player player2 = GetComponent<TeamController>().GetPlayer(2);
        Camera mainCam = Camera.main;
        mainCam.GetComponent<PlayerComponent>().player = player1;

        GameObject newCube = Instantiate(cube);
        GameObject newSphere = Instantiate(sphere);
        GameObject newTetra = Instantiate(tetra);
        GameObject newFactory = Instantiate(factory);

        newCube.GetComponent<PlayerComponent>().player = player1;
        newSphere.GetComponent<PlayerComponent>().player = player1;
        newTetra.GetComponent<PlayerComponent>().player = player2;
        newFactory.GetComponent<PlayerComponent>().player = player1;
        mainCam.GetComponent<PlayerController>().AddPlayerAsset(newCube);
        mainCam.GetComponent<PlayerController>().AddPlayerAsset(newSphere);
        //mainCam.GetComponent<PlayerController>().AddPlayerAsset(newTetra);
        mainCam.GetComponent<PlayerController>().AddPlayerAsset(newFactory);
    }

    private void FixedUpdate()
    {
        
    }
}
