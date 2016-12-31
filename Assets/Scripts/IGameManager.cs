using System.Collections.Generic;

public interface IGameManager
{
    IEnumerable<Ship> BlueforShips { get; }

    IEnumerable<Ship> RedforShips { get; }

    IDialog ShowDialog();
}