using UnityEngine;

public class Cube : Unit
{
    protected override AssetPool pool { get; set; }
    protected override int health { get; set; }
    protected override float speed { get; set; } = 5f;
    protected override int damage { get; set; }
    protected override int range { get; set; }

    protected override void Start()
    {
        base.Start();
        pool = GameObject.FindWithTag("CubePool").GetComponent<AssetPool>();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected void OnEnable()
    {
        
    }

    protected void OnDisable()
    {
        
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
