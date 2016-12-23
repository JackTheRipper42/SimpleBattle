using System.Linq;
using UnityEngine;

public class IdiotAI : AI
{
    public IdiotAI(GameManager gameManager)
        : base(gameManager)
    {
    }

    protected override void CalculateAction(Ship redforShip)
    {
        if (redforShip.Health/redforShip.MaxHealth > 0.6f)
        {
            AttackPlayer(redforShip);
        }
        else
        {
            if (Random.value > 0.3f)
            {
                AttackPlayer(redforShip);
            }
            else
            {
                redforShip.SetAction(Actions.Repair);
            }
        }
    }

    private static void AttackPlayer(Ship redforShip)
    {
        var target = redforShip.GetAvailableTargets()
            .Where(ship => ship.IsAlive)
            .OrderBy(ship => ship.Health)
            .FirstOrDefault();
        if (target != null)
        {
            redforShip.SetTarget(target);
            redforShip.SetAction(Actions.Attack);
        }
    }
}