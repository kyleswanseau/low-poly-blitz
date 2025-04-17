using UnityEngine;

public class Mine : Building
{
    [SerializeField] protected static readonly float INCOME = 5f;

    public override float MAX_HEALTH { get; } = 20f;
    public override float RANGE { get; } = 10f;
    public override float BUILD_COST { get; } = 20f;
    public override float BUILD_TIME { get; } = 10f;

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
        if (Time.fixedTime + Time.fixedDeltaTime > Mathf.Ceil(Time.fixedTime))
        {
            // Add income every second
            GetComponent<PlayerComponent>().player.team.poly += INCOME;
        }
    }
}
