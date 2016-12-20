using UnityEngine;

public abstract class Ship : MonoBehaviour
{
    protected const float MaxHealth = 100;
    protected const float MaxDamage = MaxHealth;
    protected const float MaxRepairSpeed = MaxHealth;

    [Range(0, MaxHealth)] public float Health = MaxHealth;

    [Range(0, MaxDamage)] public float Damage = MaxDamage/8;

    [Range(0, MaxRepairSpeed)] public float RepairSpeed = MaxRepairSpeed/20;

    public string ShipName;

    public float HealthBarWidth = 100f;

    public Texture HealthBarTexture;

    private float _startHealth;

    public bool IsAlive
    {
        get { return Health > 0; }
    }

    public bool IsDamaged
    {
        get { return Health < _startHealth; }
    }

    protected GameManager GameManager { get; private set; }

    public abstract void CalculateRound();

    protected virtual void Attack(Ship target)
    {
        target.Health -= Damage;
    }

    protected virtual void Repair()
    {
        Health = Mathf.Min(Health + RepairSpeed, MaxHealth);
    }

    protected virtual void Start()
    {
        _startHealth = Health;
        GameManager = FindObjectOfType<GameManager>();
    }

    protected virtual void Update()
    {
        var childRenderer = GetComponentInChildren<Renderer>();
        childRenderer.material.color = Color.Lerp(Color.red, Color.green, Health/_startHealth);
    }
}