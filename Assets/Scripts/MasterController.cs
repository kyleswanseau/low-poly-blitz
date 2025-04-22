using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UIElements;

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
        AssetPool pylonPool = GameObject.FindWithTag("PylonPool").GetComponent<AssetPool>();

        Asset player1Pylon = pylonPool.Get();
        player1Pylon.transform.position = new Vector3(-80f, 3.75f, -80f);
        player1Pylon.GetComponent<PlayerComponent>().player = player1;
        player1.AddPlayerAsset(player1Pylon);

        Asset player2Pylon = pylonPool.Get();
        player2Pylon.transform.position = new Vector3(-60f, 3.75f, -60f);
        player2Pylon.GetComponent<PlayerComponent>().player = player2;
        player2.AddPlayerAsset(player2Pylon);

        foreach (var team in teamController.GetTeams())
        {
            team.Value.AddIncome(5f);
        }
    }

    private void FixedUpdate()
    {
        if (Time.fixedTime + Time.fixedDeltaTime > Mathf.Ceil(Time.fixedTime))
        {
            foreach (var kvp in teamController.GetTeams())
            {
                if (kvp.Value.index > 0)
                {
                    bool hasBuildings = false;
                    foreach (var kvp2 in kvp.Value.GetPlayers())
                    {
                        hasBuildings = kvp2.Value.assets.Any(asset => asset is Building);
                    }
                    if (!hasBuildings)
                    {
                        Debug.Log("Team " + kvp.Value.index + " has lost!");
                    }
                    kvp.Value.AddPoly(kvp.Value.income);
                }
            }
        }
    }
}
