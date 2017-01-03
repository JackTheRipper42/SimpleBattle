using System;
using System.Collections;
using UnityEngine;

public abstract class Mission : MonoBehaviour
{
    private GameManager _gameManager;
    private bool _baseCalled;
    
    protected GameManager GameManager
    {
        get { return _gameManager ?? (_gameManager = FindObjectOfType<GameManager>()); }
    }

    protected virtual IEnumerator StartMission()
    {
        _baseCalled = true;
        yield break;
    }

    protected virtual IEnumerator EndMission()
    {
        _baseCalled = true;
        yield break;
    }

    protected virtual IEnumerator BeforeBlueforCalculation()
    {
        _baseCalled = true;
        yield break;
    }

    protected virtual IEnumerator AfterBlueforCalculation()
    {
        _baseCalled = true;
        yield break;
    }

    protected virtual IEnumerator BeforeRedforCalculation()
    {
        _baseCalled = true;
        yield break;
    }

    protected virtual IEnumerator AfterRedforCalculation()
    {
        _baseCalled = true;
        yield break;
    }

    public IEnumerator OnStartMission()
    {
        _baseCalled = false;
        yield return StartMission();
        if (!_baseCalled)
        {
            throw new InvalidOperationException("The method base.StartMission is not called.");
        }
    }

    public IEnumerator OnEndMission()
    {
        _baseCalled = false;
        yield return EndMission();
        if (!_baseCalled)
        {
            throw new InvalidOperationException("The method base.EndMission is not called.");
        }
    }

    public IEnumerator OnBeforeBlueforCalculation()
    {
        _baseCalled = false;
        yield return BeforeBlueforCalculation();
        if (!_baseCalled)
        {
            throw new InvalidOperationException("The method base.BeforeBlueforCalculation is not called.");
        }
    }

    public IEnumerator OnAfterBlueforCalculation()
    {
        _baseCalled = false;
        yield return AfterBlueforCalculation();
        if (!_baseCalled)
        {
            throw new InvalidOperationException("The method base.AfterBlueforCalculation is not called.");
        }
    }

    public IEnumerator OnBeforeRedforCalculation()
    {
        _baseCalled = false;
        yield return BeforeRedforCalculation();
        if (!_baseCalled)
        {
            throw new InvalidOperationException("The method base.BeforeRedforCalculation is not called.");
        }
    }

    public IEnumerator OnAfterRedforCalculation()
    {
        _baseCalled = false;
        yield return AfterRedforCalculation();
        if (!_baseCalled)
        {
            throw new InvalidOperationException("The method base.AfterRedforCalculation is not called.");
        }
    }
}