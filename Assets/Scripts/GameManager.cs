using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

[Serializable]
public class GameManager : MonoBehaviour
{
    [SerializeField] private RectTransform _playerShipControlParent;
    [SerializeField] private GameObject _playerShipUIPrefab;
    [SerializeField] private Transform _shipsParent;

    private readonly List<PlayerShipUI> _playerShipControls;
    private readonly List<Ship> _ships;

    private Dialog _dialog;
    private AI _ai;
    private Mission _mission;
    private IEnumerator _calculationRoutine;
    
    public GameManager()
    {
        _playerShipControls = new List<PlayerShipUI>();
        _ships = new List<Ship>();
    }

    public IEnumerable<Ship> Ships
    {
        get { return _ships; }
    }

    public IEnumerable<Ship> BlueforShips
    {
        get { return Ships.Where(ship => ship.Side == Side.Bluefor); }
    }

    public IEnumerable<Ship> RedforShips
    {
        get { return Ships.Where(ship => ship.Side == Side.Redfor); }
    }

    public IDialog ShowDialog()
    {
        return new DialogContext(_dialog, this);
    }

    protected RectTransform PlayerShipControlParent
    {
        get { return _playerShipControlParent; }
    }

    protected GameObject PlayerShipUIPrefab
    {
        get { return _playerShipUIPrefab; }
    }

    protected Transform ShipsParent
    {
        get { return _shipsParent; }
    }

    private void Start()
    {
        _ships.Clear();
        _ships.AddRange(FindObjectsOfType<Ship>());
        foreach (var ship in _ships)
        {
            ship.transform.SetParent(ShipsParent);
        }
        var playerShips = BlueforShips.ToArray();
        _playerShipControls.Clear();
        for (var index = 0; index < playerShips.Length; index++)
        {
            _playerShipControls.Add(CreatePlayerShipControl(playerShips[index], index));
        }
        PlayerShipControlParent.gameObject.SetActive(false);
        _dialog = FindObjectOfType<Dialog>();
        _ai = new IdiotAI(this);
        _mission = FindObjectOfType<Mission>();
        
        StartCoroutine(StartMission());
    }

    private PlayerShipUI CreatePlayerShipControl(Ship playerShip, int index)
    {
        var obj = Instantiate(PlayerShipUIPrefab);
        obj.transform.SetParent(PlayerShipControlParent);
        var rectTransform = (RectTransform) obj.transform;
        rectTransform.anchoredPosition = new Vector2(0, -200 + index*-60);
        var playerShipControl = obj.GetComponent<PlayerShipUI>();
        playerShipControl.Ship = playerShip;
        return playerShipControl;
    }

    public void RoundClicked()
    {
        _calculationRoutine = CalculateRound();
        StartCoroutine(_calculationRoutine);
    }

    private IEnumerator CalculateRound()
    {
        yield return _mission.OnBeforeBlueforCalculation();
        UpdateShips(BlueforShips);
        yield return _mission.OnAfterBlueforCalculation();

        _ai.CalculateActions();

        yield return _mission.OnBeforeRedforCalculation();
        UpdateShips(RedforShips);
        yield return _mission.OnAfterRedforCalculation();
    }

    private void UpdateShips(IEnumerable<Ship> ships)
    {
        foreach (var ship in ships)
        {
            ship.CalculateRound();
        }
        CheckBattleProgress();
        CleanUp();
    }

    private void CheckBattleProgress()
    {
        if (BlueforShips.All(ship => !ship.IsAlive))
        {
            StartCoroutine(FinishMission(NextScene.GameOver));
        }
        if (RedforShips.All(ship => !ship.IsAlive))
        {
            StartCoroutine(FinishMission(NextScene.MainMenu));
        }
    }

    private void CleanUp()
    {
        var deadPlayerShips = BlueforShips.Where(ship => !ship.IsAlive).ToList();
        foreach (var ship in deadPlayerShips)
        {
            var playerShipControl = _playerShipControls.Single(control => control.Ship == ship);
            _playerShipControls.Remove(playerShipControl);
            Destroy(playerShipControl.gameObject);

            _ships.Remove(ship);
            Destroy(ship.gameObject);
        }

        var deadEnemyShips = RedforShips.Where(ship => !ship.IsAlive).ToList();
        foreach (var ship in deadEnemyShips)
        {
            _ships.Remove(ship);
            Destroy(ship.gameObject);
        }
    }

    private IEnumerator StartMission()
    {
        yield return _mission.OnStartMission();
        PlayerShipControlParent.gameObject.SetActive(true);
    }

    private IEnumerator FinishMission(NextScene nextScene)
    {
        StopCoroutine(_calculationRoutine);
        PlayerShipControlParent.gameObject.SetActive(false);
        yield return _mission.OnEndMission();
        var loader = FindObjectOfType<Loader>();
        switch (nextScene)
        {
            case NextScene.MainMenu:
                loader.LoadMainMenu();
                break;
            case NextScene.GameOver:
                loader.LoadGameOver();
                break;
            default:
                throw new ArgumentOutOfRangeException("nextScene", nextScene, null);
        }
    }

    private class DialogContext : IDialog
    {
        private readonly GameManager _gameManager;
        private readonly Dialog _dialog;

        public DialogContext([NotNull] Dialog dialog, [NotNull] GameManager gameManager)
        {
            if (dialog == null)
            {
                throw new ArgumentNullException("dialog");
            }
            if (gameManager == null)
            {
                throw new ArgumentNullException("gameManager");
            }

            _dialog = dialog;
            _gameManager = gameManager;
            _gameManager.PlayerShipControlParent.gameObject.SetActive(false);
        }

        public void Dispose()
        {
            _gameManager.PlayerShipControlParent.gameObject.SetActive(true);
        }

        public ChoiseResult ShowChoices(params string[] choices)
        {
            return _dialog.ShowChoices(choices);
        }

        public CustomYieldInstruction ShowMessage(string message)
        {
            return _dialog.ShowMessage(message);
        }
    }

    private enum NextScene
    {
        MainMenu,
        GameOver
    }
}
