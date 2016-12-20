using System;

public class PlayerShip : Ship
{
    public EnemyShip Target { get; set; }

    public Actions Action { get; set; }

    public override void CalculateRound()
    {
        switch (Action)
        {
            case Actions.Attack:
                Attack(Target);
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