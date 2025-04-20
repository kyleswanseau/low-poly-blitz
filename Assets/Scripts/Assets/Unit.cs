using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public abstract class Unit : Asset
{
    protected NavMeshAgent _agent;

    public abstract float SPEED { get; }
    public abstract float DAMAGE { get; }
    public abstract float COOLDOWN { get; }

    protected abstract float cooldown { get; set; }
    protected abstract Asset? attackTarget { get; set; }
    protected abstract Vector3? moveTarget { get; set; }
    protected bool chaseEnemies { get; set; } = true;

    protected override void Start()
    {
        base.Start();
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = SPEED;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        List<Asset> enemies = NearbyEnemies();
        Asset? closestEnemy = null;
        if (enemies.Any())
        {
            Dictionary<Asset, float> assetDistance =                            // Funny way to map asset to distance
                enemies.ToDictionary(
                    key => key,
                    value => Vector3.Distance(
                        gameObject.transform.position,
                        value.gameObject.transform.position));
            closestEnemy =
                (from kvp in assetDistance
                 orderby kvp.Value ascending
                 select kvp.Key).First();
        }
        if (chaseEnemies)
        {
            if (null != attackTarget)
            {
                // Attack
                moveTarget = attackTarget.transform.position;
            }
            else if (enemies.Any())
            {
                // AttackMove / Stop and has enemies in range
                attackTarget = closestEnemy;
                moveTarget = closestEnemy.transform.position;
            }
            else
            {
                // AttackMove / Stop and does not have enemies in range
                // Keep state
            }
        }
        else
        {
            // Move
            if (Vector3.Distance(transform.position, moveTarget.Value) <= 0.5f)
            {
                StopCmd();
            }
            if (enemies.Any())
            {
                attackTarget = closestEnemy;
            }
        }
        Attack();
        Move();
        if (cooldown < COOLDOWN)
        {
            cooldown += Time.fixedDeltaTime;
        }
    }

    protected virtual void Attack()
    {
        if (null != attackTarget)
        {
            if (!attackTarget.gameObject.activeInHierarchy)
            {
                attackTarget = null;
            }
            if (CanAttackTarget() && cooldown >= COOLDOWN)
            {
                attackTarget.Damage(DAMAGE);
                cooldown = 0f;
            }
            if (chaseEnemies)
            {
                moveTarget = attackTarget.transform.position;
            }
            if (!attackTarget.gameObject.activeInHierarchy)
            {
                if (chaseEnemies)
                {
                    StopCmd();
                }
                else
                {
                    attackTarget = null;
                }
            }
        }
    }

    protected virtual void Move()
    {
        if (null != moveTarget)
        {
            _agent.SetDestination(moveTarget.Value);
        }
    }

    public virtual void AttackCmd(Asset asset)
    {
        attackTarget = asset;
        moveTarget = asset.transform.position;
        chaseEnemies = true;
    }

    public virtual void AttackMoveCmd(Vector3 position)
    {
        attackTarget = null;
        moveTarget = position;
        chaseEnemies = true;
    }

    public virtual void MoveCmd(Vector3 position)
    {
        attackTarget = null;
        moveTarget = position;
        chaseEnemies = false;
    }

    public virtual void StopCmd()
    {
        attackTarget = null;
        moveTarget = null;
        chaseEnemies = true;
        _agent.ResetPath();
    }

    protected bool CanAttackTarget()
    {
        return (Vector3.Distance(
            gameObject.transform.position,
            attackTarget.gameObject.transform.position) <= GetRange());
    }

    protected List<Asset> NearbyEnemies()
    {
        List<Asset> enemies = new List<Asset>();
        Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, GetRange());
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
