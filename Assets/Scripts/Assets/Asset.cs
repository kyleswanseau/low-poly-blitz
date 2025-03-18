using UnityEngine;

public abstract class Asset : MonoBehaviour
{
    public const float IDLE_INTENSITY = 0f;
    public const float HOVER_INTENSITY = 20f;
    public const float SELECT_INTENSITY = 100f;

    protected Camera _mainCam;
    protected Light _halo;

    protected abstract float MAX_HEALTH { get; }

    protected abstract AssetPool pool { get; set; }
    protected abstract float health { get; set; }

    protected virtual void Start()
    {
        _mainCam = Camera.main;
        _halo = gameObject.GetComponentInChildren<Light>();
        _halo.intensity = 0;
        _halo.enabled = false;
    }

    protected virtual void FixedUpdate()
    {
        
    }

    public virtual void Damage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        pool.Release(gameObject);
        throw new System.Exception("remove dead assets from selection");
    }

    public virtual void Reset()
    {
        health = MAX_HEALTH;
    }

    public void SetHalo(float intensity)
    {
        Light halo = GetComponentInChildren<Light>();
        halo.intensity = intensity;
        halo.enabled = (intensity > 0);
    }
}
