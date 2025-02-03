using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class UnitComponent : AssetComponent
{
    protected NavMeshAgent agent;

    protected void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
    }

    protected void Update()
    {
        if (isSelected && Input.GetMouseButtonDown(1))
        {
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                MoveTo(hit.point);
            }
        }
    }

    public void MoveTo(Vector3 position)
    {
        agent.SetDestination(position);
    }
}
