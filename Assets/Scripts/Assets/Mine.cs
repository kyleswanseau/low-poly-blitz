using UnityEngine;

public class Mine : Building
{
    [SerializeField] public static readonly float INCOME = 5f;

    [SerializeField] public static float MAX_HEALTH = 20f;
    [SerializeField] public static float RANGE = 10f;
    [SerializeField] public static float BUILD_COST = 20f;
    [SerializeField] public static float BUILD_TIME = 10f;

    protected override AssetPool pool { get; set; }
    protected override float health { get; set; } = 20f;

    protected override void Start()
    {
        base.Start();
        pool = GameObject.FindWithTag("MinePool").GetComponent<AssetPool>();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void Die()
    {
        GetComponent<PlayerComponent>().player.team.AddIncome(-INCOME);
        base.Die();
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
