using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShipUI : MonoBehaviour
{
    public PlayerShip PlayerShip;
    public Text ShipNameText;
    public Dropdown ActionDropdown;
    public Dropdown TargetDropdown;

    private GameManager _gameManager;
    private List<EnemyShip> _currentEnemyShips;
    private List<Actions> _actions;

    private Actions Action
    {
        get
        {
            return _actions[ActionDropdown.value];
        }
    }

    protected virtual void Start()
    {
        _actions = new List<Actions>();
        _gameManager = FindObjectOfType<GameManager>();
        ShipNameText.text = PlayerShip.ShipName;

        UpdateActions();
        UpdateTargetList();
    }

    protected virtual void Update()
    {
        if (!_currentEnemyShips.SequenceEqual(_gameManager.EnemyShips))
        {
            UpdateTargetList();
        }
        UpdateActions();
    }

    private void UpdateActions()
    {
        _actions.Clear();
        _actions.Add(Actions.Attack);
        if (PlayerShip.IsDamaged)
        {
            _actions.Add(Actions.Repair);
        }
        ActionDropdown.onValueChanged.RemoveAllListeners();
        ActionDropdown.ClearOptions();
        ActionDropdown.AddOptions(_actions.Select(action => action.ToString()).ToList());
        ActionDropdown.onValueChanged.AddListener(ActionIndexChanged);
    }

    private void UpdateTargetList()
    {
        _currentEnemyShips = _gameManager.EnemyShips.ToList();

        TargetDropdown.onValueChanged.RemoveListener(TargetIndexChanged);
        TargetDropdown.ClearOptions();
        TargetDropdown.AddOptions(_currentEnemyShips.Select(ship => ship.ShipName).ToList());

        if (PlayerShip.Target != null)
        {
            var index = _currentEnemyShips.IndexOf(PlayerShip.Target);
            TargetDropdown.value = Math.Max(0, index);
        }
        else
        {
            TargetDropdown.value = 0;
        }

        TargetIndexChanged(TargetDropdown.value);
        TargetDropdown.onValueChanged.AddListener(TargetIndexChanged);
    }
     
    private void ActionIndexChanged(int index)
    {
        PlayerShip.Action = Action;
        switch (Action)
        {
            case Actions.Attack:
                TargetDropdown.gameObject.SetActive(true);
                break;
            case Actions.Repair:
                TargetDropdown.gameObject.SetActive(false);
                break;
            default:
                throw new NotSupportedException();
        }
    }

    private void TargetIndexChanged(int index)
    {
        PlayerShip.Target = _currentEnemyShips.Count > 0 ? _currentEnemyShips[index] : null;
    }
}
