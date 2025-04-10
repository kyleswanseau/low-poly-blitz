using UnityEngine;

public class Pylon : Building
{
    protected override float MAX_HEALTH { get; } = 20f;
    protected float RANGE { get; } = 10f;
    public override float BUILD_COST { get; } = 10f;
    public override float BUILD_TIME { get; } = 5f;

    protected override AssetPool pool { get; set; }
    protected override float health { get; set; } = 20f;

    protected override void Start()
    {
        base.Start();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
