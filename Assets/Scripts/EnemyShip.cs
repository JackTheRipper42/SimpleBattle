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

    protected override void Start()
    {
        base.Start();       
        var text = GetComponentInChildren<Text>();
        text.text = ShipName;
    }
}
