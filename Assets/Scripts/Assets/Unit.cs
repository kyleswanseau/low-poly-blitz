using UnityEngine;
using UnityEngine.AI;

public abstract class Unit : Asset
{
    protected NavMeshAgent _agent;

    protected abstract float speed { get; set; }
    protected abstract int damage { get; set; }
    protected abstract int range { get; set; }

    protected override void Start()
    {
        base.Start();
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = speed;
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

    protected override void FixedUpdate()
    {
        /*
        if (isSelected && Input.GetMouseButtonDown(1))
        {
            Debug.Log("Move out");
            Ray ray = _mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                MoveTo(hit.point);
            }
        }
        */
    }

    protected abstract void Attack(Asset asset);

    public void MoveTo(Vector3 position)
    {
        _agent.SetDestination(position);
    }
}
