using UnityEngine;
using UnityEngine.SocialPlatforms;

public class MasterController : MonoBehaviour
{
    [SerializeField] private Asset cube;
    [SerializeField] private Asset sphere;
    [SerializeField] private Asset tetra;
    [SerializeField] private Asset factory;
    [SerializeField] private Asset mine;
    [SerializeField] private Asset pylon;
    private TeamController teamController;

    private void Start()
    {
        teamController = GetComponent<TeamController>();
        Player player1 = teamController.GetPlayer(1);
        Player player2 = teamController.GetPlayer(2);
        Camera mainCam = Camera.main;
        mainCam.GetComponent<PlayerComponent>().player = player1;

        /*
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
        */
        Asset newPylon = Instantiate(pylon);
        newPylon.GetComponent<PlayerComponent>().player = player1;
        player1.AddPlayerAsset(newPylon);

        foreach (var team in teamController.GetTeams())
        {
            team.Value.AddIncome(5f);
        }
    }

    private void FixedUpdate()
    {
        if (Time.fixedTime + Time.fixedDeltaTime > Mathf.Ceil(Time.fixedTime))
        {
            foreach (var team in teamController.GetTeams())
            {
                team.Value.AddPoly(team.Value.income);
            }
        }
    }
}
