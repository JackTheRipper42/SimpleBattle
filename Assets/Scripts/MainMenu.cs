using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button StartButton;
    public Button ExitButton;

    private void Start()
    {
        StartButton.onClick.AddListener(StartClicked);
        ExitButton.onClick.AddListener(ExitClicked);
    }

    private void StartClicked()
    {
        FindObjectOfType<Loader>().LoadTestMission();
    }

    private void ExitClicked()
    {
        Application.Quit();
    }
}
