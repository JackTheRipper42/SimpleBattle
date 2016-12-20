using System;
using JetBrains.Annotations;
using UnityEngine;

public class ChoiseResult
{
    private readonly CustomYieldInstruction _yieldInstruction;
    private readonly Func<int> _choise;


    public ChoiseResult([NotNull] CustomYieldInstruction yieldInstruction, [NotNull] Func<int> choise)
    {
        if (yieldInstruction == null)
        {
            throw new ArgumentNullException("yieldInstruction");
        }
        if (choise == null)
        {
            throw new ArgumentNullException("choise");
        }

        _yieldInstruction = yieldInstruction;
        _choise = choise;
    }

    public int Choise
    {
        get { return _choise(); }
    }

    public CustomYieldInstruction YieldInstruction
    {
        get { return _yieldInstruction; }
    }
}