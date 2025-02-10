using UnityEngine;

public class MasterController : MonoBehaviour
{
    public GameObject cube;
    public GameObject sphere;
    public GameObject tetra;
    public GameObject factory;

    private void Start()
    {
        Player player0 = GetComponent<TeamController>().GetPlayer(0);
        Player player1 = GetComponent<TeamController>().GetPlayer(1);
        Camera mainCam = Camera.main;
        mainCam.GetComponent<PlayerComponent>().SetPlayer(player0);

        GameObject newCube = Instantiate(cube);
        GameObject newSphere = Instantiate(sphere);
        GameObject newTetra = Instantiate(tetra);
        GameObject newFactory = Instantiate(factory);

        newCube.GetComponent<PlayerComponent>().SetPlayer(player0);
        newSphere.GetComponent<PlayerComponent>().SetPlayer(player0);
        newTetra.GetComponent<PlayerComponent>().SetPlayer(player1);
        newFactory.GetComponent<PlayerComponent>().SetPlayer(player0);
        mainCam.GetComponent<PlayerController>().AddPlayerAsset(newCube);
        mainCam.GetComponent<PlayerController>().AddPlayerAsset(newSphere);
        //mainCam.GetComponent<PlayerController>().AddPlayerAsset(newTetra);
        mainCam.GetComponent<PlayerController>().AddPlayerAsset(newFactory);

        Debug.Log(newTetra.GetComponent<PlayerComponent>().GetPlayer().index);
    }

    private void Update()
    {
        
    }
}
