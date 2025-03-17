using UnityEngine;

public class Sphere : Unit
{
    protected override AssetPool pool { get; set; }
    protected override int health { get; set; }
    protected override float speed { get; set; } = 10f;
    protected override int damage { get; set; }
    protected override int range { get; set; }

    protected override void Start()
    {
        base.Start();
        pool = GameObject.FindWithTag("SpherePool").GetComponent<AssetPool>();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void Attack(Asset asset)
    {
        throw new System.NotImplementedException();
    }

    protected override void CheckHealth()
    {
        if (health <= 0)
        {
            pool.Release(gameObject);
        }
    }
}
