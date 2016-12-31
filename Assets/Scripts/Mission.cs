using JetBrains.Annotations;
using System;
using System.Collections;

public abstract class Mission
{
    private readonly IGameManager _gameManager;

    private bool _baseCalled;

    protected Mission([NotNull] IGameManager gameManager)
    {
        if (gameManager == null)
        {
            throw new ArgumentNullException("gameManager");
        }

        _gameManager = gameManager;
    }

    protected IGameManager GameManager
    {
        get { return _gameManager; }
    }


    protected virtual IEnumerator Start()
    {
        _baseCalled = true;
        yield break;
    }

    protected virtual IEnumerator End()
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

    public IEnumerator OnStart()
    {
        _baseCalled = false;
        yield return Start();
        if (!_baseCalled)
        {
            throw new InvalidOperationException("The method base.Start is not called.");
        }
    }

    public IEnumerator OnEnd()
    {
        _baseCalled = false;
        yield return End();
        if (!_baseCalled)
        {
            throw new InvalidOperationException("The method base.End is not called.");
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