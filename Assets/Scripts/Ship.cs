using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Ship : MonoBehaviour
{
    [SerializeField] private float _maxHealth = 100f;
    [SerializeField] private float _damage = 12.5f;
    [SerializeField] private float _repairSpeed = 5f;
    [SerializeField] private string _shipName;
    [SerializeField] private Side _side;

    private bool _initialized;

    public float MaxHealth
    {
        get { return _maxHealth; }
    }

    public float Damage
    {
        get { return _damage; }
    }

    public float RepairSpeed
    {
        get { return _repairSpeed; }
    }

    public string ShipName
    {
        get { return _shipName; }
    }
    
    public Side Side
    {
        get { return _side; }
    }

    public bool IsAlive
    {
        get { return Health > 0; }
    }

    public float Health { get; private set; }

    public Ship Target { get; private set; }

    public Actions Action { get; private set; }

    protected GameManager GameManager { get; private set; }

    public virtual void Initialize(ShipDescription description)
    {
        if (_initialized)
        {
            throw new InvalidOperationException("The ship is already initialized.");
        }

        _maxHealth = description.MaxHealth;
        _damage = description.Damage;
        _repairSpeed = description.RepairSpeed;
        _shipName = description.ShipName;
        _side = description.Side;

        _initialized = true;
    }

    public virtual void CalculateRound()
    {
        switch (Action)
        {
            case Actions.Attack:
                if (Target != null && Target.IsAlive)
                {
                    Attack(Target);
                }
                break;
            case Actions.Repair:
                Repair();
                break;
            default:
                throw new NotSupportedException();
        }
    }

    public virtual IEnumerable<Ship> GetAvailableTargets()
    {
        return GameManager.Ships.Where(ship => ship.Side != Side);
    }

    public virtual bool CanRepair()
    {
        return Health < MaxHealth;
    }

    public virtual bool CanAttack()
    {
        return true;
    }

    public virtual void SetTarget(Ship target)
    {
        Target = target;
    }

    public void SetAction(Actions action)
    {
        switch (action)
        {
            case Actions.Attack:
                if (!CanAttack())
                {
                    throw new InvalidOperationException("The ship cannot attack.");
                }
                break;
            case Actions.Repair:
                if (!CanRepair())
                {
                    throw new InvalidOperationException("The ship cannot repair.");
                }
                break;
            default:
                throw new ArgumentOutOfRangeException("action", action, null);
        }
        Action = action;
    }

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
        _initialized = true;
        Health = MaxHealth;
        GameManager = FindObjectOfType<GameManager>();
        var text = GetComponentInChildren<Text>();
        text.text = ShipName;
        switch (Side)
        {
            case Side.Bluefor:
                transform.name = string.Format("Player - {0}", ShipName);
                break;
            case Side.Redfor:
                transform.name = string.Format("Enemy - {0}", ShipName);
                break;
            default:
                throw new NotSupportedException();
        }
    }

    protected virtual void Update()
    {
        var childRenderer = GetComponentInChildren<Renderer>();
        childRenderer.material.color = Color.Lerp(Color.red, Color.green, Health/MaxHealth);
    }
}