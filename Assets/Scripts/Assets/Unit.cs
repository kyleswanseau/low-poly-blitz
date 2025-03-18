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

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected abstract void Attack(Asset asset);

    public void MoveTo(Vector3 position)
    {
        _agent.SetDestination(position);
    }
}
