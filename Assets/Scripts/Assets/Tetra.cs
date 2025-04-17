using UnityEngine;

public class Tetra : Unit
{
    public override float MAX_HEALTH { get; } = 10f;
    public override float SPEED { get; } = 3f;
    public override float DAMAGE { get; } = 10f;
    public override float COOLDOWN { get; } = 5f;
    public override float RANGE { get; } = 10f;
    public override float BUILD_COST { get; } = 20f;
    public override float BUILD_TIME { get; } = 10f;

    protected override AssetPool pool { get; set; }
    protected override float health { get; set; } = 5f;
    protected override float cooldown { get; set; } = 5f;
    protected override Asset? attackTarget { get; set; }
    protected override Vector3? moveTarget { get; set; }

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
