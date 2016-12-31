using System.Collections;
using JetBrains.Annotations;

public class TestMission : Mission
{
    public TestMission([NotNull] IGameManager gameManager) 
        : base(gameManager)
    {
    }

    protected override IEnumerator Start()
    {
        using (var dialog
            = GameManager.ShowDialog())
        {
            yield return dialog.ShowMessage("This is a simple battle.");
            var choices = new[] {"A", "B", "C"};
            var result = dialog.ShowChoices(choices);
            yield return result.YieldInstruction;
            yield return dialog.ShowMessage(string.Format("You have selected '{0}'.", choices[result.Choise]));
        }
        yield return base.Start();
    }
}