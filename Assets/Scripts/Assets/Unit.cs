using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public abstract class Unit : Asset
{
    protected NavMeshAgent _agent;

    protected abstract float SPEED { get; }
    protected abstract float DAMAGE { get; }
    protected abstract float COOLDOWN { get; }
    protected abstract float RANGE { get; }

    public abstract Asset? target { get; set; }
    protected bool ignoreEnemies { get; set; } = false;

    protected override void Start()
    {
        base.Start();
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = SPEED;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (ignoreEnemies)
        {
            if (null != target)
            {
                // Has target
                Attack();
            }
            else
            {
                // No target
                // Nothing to see here, move along now
            }
        }
        else
        {
            List<Asset> enemies = NearbyEnemies();
            if (enemies.Any())
            {
                // Has enemies in range
                Dictionary<Asset, float> assetDistance                          // Funny way to map asset to distance
                    = enemies.ToDictionary(
                        key => key,
                        value => Vector3.Distance(
                            gameObject.transform.position,
                            value.gameObject.transform.position));
                Attack((from kvp in assetDistance
                        orderby kvp.Value ascending
                        select kvp.Key).First());
            }
            else
            {
                // No enemies in range
                // Nothing to see here, move along now
            }
        }
        if (_agent.remainingDistance <= 0.5f)
        {
            Stop();
        }
    }

    protected virtual void Attack()
    {
        if (!target.gameObject.activeInHierarchy)
        {
            target = null;
        }
        if (null != target)
        {
            ignoreEnemies = true;
            if (CanAttackTarget())
            {
                target.Damage(DAMAGE);
            }
            else
            {
                Move(target.transform.position);
            }
        }
    }

    public virtual void Attack(Asset asset)
    {
        target = asset;
        Attack();
    }

    public virtual void AttackMove(Vector3 position)
    {
        Move(position);
        ignoreEnemies = false;
    }

    public virtual void Move(Vector3 position)
    {
        ignoreEnemies = true;
        _agent.SetDestination(position);
    }

    public virtual void Stop()
    {
        ignoreEnemies = false;
        target = null;
        _agent.ResetPath();
    }

    protected bool CanAttackTarget()
    {
        return (Vector3.Distance(
            gameObject.transform.position,
            target.gameObject.transform.position) <= RANGE);
    }

    protected List<Asset> NearbyEnemies()
    {
        List<Asset> enemies = new List<Asset>();
        Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, RANGE);
        foreach (Collider hitCollider in hitColliders)
        {
            GameObject assetObj = hitCollider.gameObject;
            if (assetObj.GetComponent<Asset>()
                && (assetObj.GetComponent<PlayerComponent>().player.team
                != GetComponent<PlayerComponent>().player.team))
            {
                enemies.Add(assetObj.GetComponent<Asset>());
            }
        }
        return enemies;
    }
}
