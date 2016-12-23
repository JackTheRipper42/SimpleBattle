using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public RectTransform PlayerShipControlParent;
    public Button RoundButton;
    public string MainSceneName;
    public string GameOverSceneName;
    public GameObject PlayerShipUIPrefab;

    private readonly List<PlayerShipUI> _playerShipControls;
    private readonly List<Ship> _ships;

    private Dialog _dialog;
    private AI _ai;

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


    private void Start()
    {
        RoundButton.onClick.AddListener(RoundClicked);

        _ships.Clear();
        _ships.AddRange(FindObjectsOfType<Ship>());
        var playerShips = BlueforShips.ToArray();
        _playerShipControls.Clear();
        for (var index = 0; index < playerShips.Length; index++)
        {
            _playerShipControls.Add(CreatePlayerShipControl(playerShips[index], index));
        }
        _dialog = FindObjectOfType<Dialog>();
        _ai = new IdiotAI(this);

        StartCoroutine(StartMission());
    }

    private IEnumerator StartMission()
    {
        PlayerShipControlParent.gameObject.SetActive(false);
        yield return new WaitForEndOfFrame();
        //yield return _dialog.ShowMessage("This is a simple battle");
        //var choices = new[] { "A", "B", "C" };
        //var result = _dialog.ShowChoices(choices);
        //yield return result.YieldInstruction;
        //yield return _dialog.ShowMessage(string.Format("You have selected '{0}'", choices[result.Choise]));
        PlayerShipControlParent.gameObject.SetActive(true);
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

    private void RoundClicked()
    {
        StartCoroutine(CalculateRound());
    }

    private IEnumerator CalculateRound()
    {
        UpdateShips(BlueforShips);

        yield return new WaitForSecondsRealtime(0.001f);

        _ai.CalculateActions();
        UpdateShips(RedforShips);

        yield return new WaitForSecondsRealtime(0.001f);
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
            GameOver();
        }
        if (RedforShips.All(ship => !ship.IsAlive))
        {
            BattleFinished();
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

    private void BattleFinished()
    {
        SceneManager.LoadScene(MainSceneName, LoadSceneMode.Single);
    }

    private void GameOver()
    {
        SceneManager.LoadScene(GameOverSceneName, LoadSceneMode.Single);
    }
}
