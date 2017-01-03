using System.Collections;

public class TestMission : Mission
{
    private int _round;

    protected override IEnumerator StartMission()
    {
        _round = 0;

        using (var dialog = GameManager.ShowDialog())
        {
            yield return dialog.ShowMessage("This is a simple battle.");
            var choices = new[] {"A", "B", "C"};
            var result = dialog.ShowChoices(choices);
            yield return result.YieldInstruction;
            yield return dialog.ShowMessage(string.Format("You have selected '{0}'.", choices[result.Choise]));
        }

        GameManager.Spawn(new ShipDescription
        {
            ShipName = "Pandora",
            MaxHealth = 100f,
            Damage = 25,
            RepairSpeed = 10,
            Side = Side.Bluefor
        });
        GameManager.Spawn(new ShipDescription
        {
            ShipName = "Pandora II",
            MaxHealth = 100f,
            Damage = 25,
            RepairSpeed = 10,
            Side = Side.Bluefor
        });
        GameManager.Spawn(new ShipDescription
        {
            ShipName = "Ajax",
            MaxHealth = 80f,
            Damage = 12,
            RepairSpeed = 5,
            Side = Side.Redfor
        });
        GameManager.Spawn(new ShipDescription
        {
            ShipName = "Sparrow",
            MaxHealth = 100f,
            Damage = 30,
            RepairSpeed = 5,
            Side = Side.Redfor
        });

        yield return base.StartMission();
    }

    protected override IEnumerator AfterBlueforCalculation()
    {
        if (_round++ == 2)
        {
            using (var dialog = GameManager.ShowDialog())
            {
                yield return dialog.ShowMessage("Enemy reinforcement arrive.");
            }

            GameManager.Spawn(new ShipDescription
            {
                ShipName = "Cyana",
                MaxHealth = 80f,
                Damage = 12,
                RepairSpeed = 5,
                Side = Side.Redfor
            });
            GameManager.Spawn(new ShipDescription
            {
                ShipName = "Beagle",
                MaxHealth = 100f,
                Damage = 10,
                RepairSpeed = 5,
                Side = Side.Redfor
            });
        }

        yield return base.AfterBlueforCalculation();
    }
}