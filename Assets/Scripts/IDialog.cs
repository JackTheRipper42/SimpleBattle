using System;
using UnityEngine;

public interface IDialog : IDisposable
{
    ChoiseResult ShowChoices(params string[] choices);
    CustomYieldInstruction ShowMessage(string message);
}