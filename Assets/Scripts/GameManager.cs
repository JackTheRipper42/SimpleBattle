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
    private readonly List<EnemyShip> _enemyShips;
    private readonly List<PlayerShip> _playerShips;

    private Dialog _dialog;

    public GameManager()
    {
        _enemyShips = new List<EnemyShip>();
        _playerShips = new List<PlayerShip>();
        _playerShipControls = new List<PlayerShipUI>();
    }

    public List<EnemyShip> EnemyShips
    {
        get { return _enemyShips; }
    }

    public List<PlayerShip> PlayerShips
    {
        get { return _playerShips; }
    }

    private void Start()
    {
        RoundButton.onClick.AddListener(RoundClicked);

        PlayerShips.Clear();
        PlayerShips.AddRange(FindObjectsOfType<PlayerShip>());
        _playerShipControls.Clear();
        for (var index = 0; index < PlayerShips.Count; index++)
        {
            _playerShipControls.Add(CreatePlayerShipControl(PlayerShips[index], index));
        }
        EnemyShips.Clear();
        EnemyShips.AddRange(FindObjectsOfType<EnemyShip>());
        _dialog = FindObjectOfType<Dialog>();

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

    private PlayerShipUI CreatePlayerShipControl(PlayerShip playerShip, int index)
    {
        var obj = Instantiate(PlayerShipUIPrefab);
        obj.transform.SetParent(PlayerShipControlParent);
        var rectTransform = (RectTransform) obj.transform;
        rectTransform.anchoredPosition = new Vector2(0, -200 + index * -60);
        var playerShipControl = obj.GetComponent<PlayerShipUI>();
        playerShipControl.PlayerShip = playerShip;
        return playerShipControl;
    }

    private void RoundClicked()
    {
        StartCoroutine(CalculateRound());
    }

    private IEnumerator CalculateRound()
    {
        foreach (var playerShip in _playerShips)
        {
            playerShip.CalculateRound();
        }

        CheckBattleProgress();
        CleanUp();

        yield return new WaitForSecondsRealtime(0.001f);

        foreach (var enemyShip in EnemyShips)
        {
            enemyShip.CalculateRound();
        }

        CheckBattleProgress();
        CleanUp();

        yield return new WaitForSecondsRealtime(0.001f);
    }

    private void CheckBattleProgress()
    {
        if (_playerShipControls.All(control => !control.PlayerShip.IsAlive))
        {
            GameOver();
        }
        if (EnemyShips.All(ship => !ship.IsAlive))
        {
            BattleFinished();
        }
    }

    private void CleanUp()
    {
        var deadPlayerShips = PlayerShips.Where(ship => !ship.IsAlive).ToList();
        foreach (var ship in deadPlayerShips)
        {
            var playerShipControl = _playerShipControls.Single(control => control.PlayerShip == ship);
            _playerShipControls.Remove(playerShipControl);
            Destroy(playerShipControl.gameObject);

            PlayerShips.Remove(ship);
            Destroy(ship.gameObject);
        }

        var deadEnemyShips = EnemyShips.Where(ship => !ship.IsAlive).ToList();
        foreach (var ship in deadEnemyShips)
        {
            EnemyShips.Remove(ship);
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
