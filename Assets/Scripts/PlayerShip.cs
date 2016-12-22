using System;

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
}