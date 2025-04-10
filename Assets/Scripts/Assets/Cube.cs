using Unity.VisualScripting;
using UnityEngine;

public class Cube : Unit
{
    protected override float MAX_HEALTH { get; } = 10f;
    protected override float SPEED { get; } = 5f;
    protected override float DAMAGE { get; } = 5f;
    protected override float COOLDOWN { get; } = 5f;
    protected override float RANGE { get; } = 1f;
    public override float BUILD_COST { get; } = 5f;
    public override float BUILD_TIME { get; } = 5f;

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

    protected void OnEnable()
    {
        
    }

    protected void OnDisable()
    {
        
    }
}
