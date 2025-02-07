using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Unit : Asset
{
    protected float moveSpeed = 0;
    protected NavMeshAgent agent;

    new protected void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
    }

    protected void Update()
    {
        if (IsSelected && Input.GetMouseButtonDown(1))
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
