public abstract class AI
{
    private readonly GameManager _gameManager;

    protected AI(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    protected GameManager GameManager
    {
        get { return _gameManager; }
    }

    public void CalculateActions()
    {
        foreach (var ship in GameManager.RedforShips)
        {
            CalculateAction(ship);
        }
    }

    protected abstract void CalculateAction(Ship ship);
}