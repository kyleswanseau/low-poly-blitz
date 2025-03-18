using UnityEngine;

public abstract class Asset : MonoBehaviour
{
    public const float IDLE_INTENSITY = 0f;
    public const float HOVER_INTENSITY = 20f;
    public const float SELECT_INTENSITY = 100f;

    protected Camera _mainCam;
    protected Light _halo;
    
    protected abstract AssetPool pool { get; set; }
    protected abstract int health { get; set; }

    protected virtual void Start()
    {
        _mainCam = Camera.main;
        _halo = gameObject.GetComponentInChildren<Light>();
        _halo.intensity = 0;
        _halo.enabled = false;
    }

    protected virtual void FixedUpdate()
    {
        //CheckHealth();
    }

    protected virtual void Damage(int damage)
    {
        health -= damage;
        CheckHealth();
    }

    protected abstract void CheckHealth();

    public void SetHalo(float intensity)
    {
        Light halo = GetComponentInChildren<Light>();
        halo.intensity = intensity;
        halo.enabled = (intensity > 0);
    }
}
