using UnityEngine;

public class Cube : Unit
{
    [SerializeField] public static float MAX_HEALTH = 10f;
    [SerializeField] public static float RANGE = 1f;
    [SerializeField] public static float BUILD_COST = 5f;
    [SerializeField] public static float BUILD_TIME = 5f;
    public override float SPEED { get; } = 5f;
    public override float DAMAGE { get; } = 5f;
    public override float COOLDOWN { get; } = 5f;

    protected override AssetPool pool { get; set; }
    protected override float health { get; set; } = 10f;
    protected override float cooldown { get; set; } = 5f;
    protected override Asset? attackTarget { get; set; }
    protected override Vector3? moveTarget { get; set; }

    protected override void Start()
    {
        base.Start();
        pool = GameObject.FindWithTag("CubePool").GetComponent<AssetPool>();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Reset()
    {
        health = MAX_HEALTH;
    }

    public override float GetRange()
    {
        return RANGE;
    }
}
