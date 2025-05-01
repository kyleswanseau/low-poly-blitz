using UnityEngine;

public abstract class Asset : MonoBehaviour
{
    protected Camera _mainCam;
    [SerializeField] protected SelectRing _halo;
    [SerializeField] protected RangeRing _range;

    protected abstract AssetPool pool { get; set; }
    protected abstract float health { get; set; }

    protected virtual void Start()
    {
        _mainCam = Camera.main;
        _halo = GetComponentInChildren<SelectRing>();
        _range = GetComponentInChildren<RangeRing>();
        SetHalo(EIntensity.Idle);
        SetRange(false);
    }

    protected virtual void FixedUpdate()
    {
    
    }

    public virtual void Damage(float damage)
    {
        health -= damage;
        if (health <= 0f)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        Player player = GetComponent<PlayerComponent>().player;
        player.RemovePlayerAsset(this);
        player.controller?.RemoveAsset(this);
        if (null != pool)
        {
            pool.Release(this);
        }
    }

    public abstract void Reset();

    public void SetHalo(EIntensity intensity)
    {
        _halo.SetIntensity(intensity);
    }

    public void SetRange(bool set)
    {
        _range.SetVisible(set);
    }

    public abstract float GetRange();
}
