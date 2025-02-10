using UnityEngine;
using UnityEngine.AI;

public class Unit : Asset
{
    protected float _moveSpeed = 0;
    protected NavMeshAgent _agent;

    new protected void Start()
    {
        base.Start();
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = _moveSpeed;
    }

    protected void Update()
    {
        if (isSelected && Input.GetMouseButtonDown(1))
        {
            Ray ray = _mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                MoveTo(hit.point);
            }
        }
    }

    public void MoveTo(Vector3 position)
    {
        _agent.SetDestination(position);
    }
}
