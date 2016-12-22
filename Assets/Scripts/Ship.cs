using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Ship : MonoBehaviour
{
    public float MaxHealth = 100f;
    public float Damage = 12.5f;
    public float RepairSpeed = 5f;
    public string ShipName;
    public float HealthBarWidth = 100f;
    public Texture HealthBarTexture;

    protected Ship(Side side)
    {
        Side = side;
    }

    public bool IsAlive
    {
        get { return Health > 0; }
    }

    public float Health { get; private set; }

    public Ship Target { get; private set; }

    public Actions Action { get; private set; }

    public Side Side { get; private set; }
    
    protected GameManager GameManager { get; private set; }

    public abstract void CalculateRound();

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
        Health = MaxHealth;
        GameManager = FindObjectOfType<GameManager>();
    }

    protected virtual void Update()
    {
        var childRenderer = GetComponentInChildren<Renderer>();
        childRenderer.material.color = Color.Lerp(Color.red, Color.green, Health/MaxHealth);
    }
}