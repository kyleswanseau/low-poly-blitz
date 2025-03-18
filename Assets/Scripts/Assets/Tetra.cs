using UnityEngine;

public class Tetra : Unit
{
    protected override float MAX_HEALTH { get; } = 10f;
    protected override float SPEED { get; } = 3f;
    protected override float DAMAGE { get; } = 10f;
    protected override float COOLDOWN { get; } = 15f;
    protected override float RANGE { get; } = 2f;

    protected override AssetPool pool { get; set; }
    protected override float health { get; set; } = 5f;
    public override Asset? target { get; set; }

    protected override void Start()
    {
        base.Start();
        pool = GameObject.FindWithTag("TetraPool").GetComponent<AssetPool>();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
