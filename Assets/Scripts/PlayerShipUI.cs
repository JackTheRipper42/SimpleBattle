using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShipUI : MonoBehaviour
{
    public Ship Ship;
    public Text ShipNameText;
    public Dropdown ActionDropdown;
    public Dropdown TargetDropdown;

    private List<Ship> _targets;
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
        ShipNameText.text = Ship.ShipName;

        UpdateActions();
        UpdateTargetList();
    }

    protected virtual void Update()
    {
        UpdateTargetList();
        UpdateActions();
    }

    private void UpdateActions()
    {
        _actions.Clear();
        if (Ship.CanAttack())
        {
            _actions.Add(Actions.Attack);
        }
        if (Ship.CanRepair())
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
        _targets = Ship.GetAvailableTargets().ToList();

        TargetDropdown.onValueChanged.RemoveListener(TargetIndexChanged);
        TargetDropdown.ClearOptions();
        TargetDropdown.AddOptions(_targets.Select(ship => ship.ShipName).ToList());

        if (Ship.Target != null)
        {
            var index = _targets.IndexOf(Ship.Target);
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
        Ship.SetAction(Action);
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
        Ship.SetTarget(_targets.Count > 0 ? _targets[index] : null);
    }
}
