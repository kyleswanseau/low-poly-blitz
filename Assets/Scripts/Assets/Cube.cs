using UnityEngine;

public class Cube : Unit
{
    protected override float MAX_HEALTH { get; } = 10f;
    protected override float SPEED { get; } = 5f;
    protected override float DAMAGE { get; } = 5f;
    protected override float COOLDOWN { get; } = 5f;
    protected override float RANGE { get; } = 1f;

    protected override AssetPool pool { get; set; }
    protected override float health { get; set; } = 10f;
    public override Asset? target { get; set; }

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
