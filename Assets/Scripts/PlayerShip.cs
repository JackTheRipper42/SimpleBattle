using System;
using System.Collections.Generic;
using System.Linq;

public class PlayerShip : Ship
{
    public PlayerShip()
        : base(Side.Bluefor)
    {        
    }

    public override void CalculateRound()
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

    protected override void Start()
    {
        base.Start();
        transform.name = string.Format("Player - {0}", ShipName);        
    }
}