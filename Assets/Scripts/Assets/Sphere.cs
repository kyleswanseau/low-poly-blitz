using UnityEngine;

public class Sphere : Unit
{
    public override float MAX_HEALTH { get; } = 5f;
    public override float SPEED { get; } = 10f;
    public override float DAMAGE { get; } = 5f;
    public override float COOLDOWN { get; } = 10f;
    public override float RANGE { get; } = 1f;
    public override float BUILD_COST { get; } = 10f;
    public override float BUILD_TIME { get; } = 5f;

    protected override AssetPool pool { get; set; }
    protected override float health { get; set; } = 5f;
    protected override float cooldown { get; set; } = 10f;
    protected override Asset? attackTarget { get; set; }
    protected override Vector3? moveTarget { get; set; }

    protected override void Start()
    {
        base.Start();
        pool = GameObject.FindWithTag("SpherePool").GetComponent<AssetPool>();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
