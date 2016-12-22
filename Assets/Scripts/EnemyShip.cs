using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EnemyShip : Ship
{
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

    public override IEnumerable<Ship> GetAvailableTargets()
    {
        return GameManager.PlayerShips.Cast<Ship>();
    }

    private void AttackPlayer()
    {
        var target = GameManager.PlayerShips
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
