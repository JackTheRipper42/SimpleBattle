using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EnemyShip : Ship
{
    public EnemyShip()
        : base(Side.Redfor)
    {        
    }

    public override void CalculateRound()
    {
        if (Health/MaxHealth > 0.6f)
        {
            AttackPlayer();
        }
        else
        {
            if (Random.value > 0.3f)
            {
                AttackPlayer();
            }
            else
            {
                Repair();
            }
        }
    }

    private void AttackPlayer()
    {
        var target = GetAvailableTargets()
            .Where(ship => ship.IsAlive)
            .OrderBy(ship => ship.Health)
            .FirstOrDefault();
        if (target != null)
        {
            Attack(target);
        }
    }

    protected override void Start()
    {
        base.Start();
        transform.name = string.Format("Enemy - {0}", ShipName);
        var text = GetComponentInChildren<Text>();
        text.text = ShipName;
    }
}
