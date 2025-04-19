using UnityEngine;

public abstract class Asset : MonoBehaviour
{
    protected Camera _mainCam;
    protected SelectRing _halo;
    protected RangeRing _range;

    public abstract float MAX_HEALTH { get; }
    public abstract float RANGE { get; }
    public abstract float BUILD_COST { get; }
    public abstract float BUILD_TIME { get; }

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

    protected virtual void Die()
    {
        GetComponent<PlayerComponent>().player.controller.RemoveAsset(this);
        pool.Release(this);
    }

    public virtual void Reset()
    {
        health = MAX_HEALTH;
    }

    public void SetHalo(EIntensity intensity)
    {
        _halo.SetIntensity(intensity);
    }

    public void SetRange(bool set)
    {
        _range.SetVisible(set);
    }
}
