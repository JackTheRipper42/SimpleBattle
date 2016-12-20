using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button StartButton;
    public Button ExitButton;
    public string BattleSceneName;

    private void Start()
    {
        StartButton.onClick.AddListener(StartClicked);
        ExitButton.onClick.AddListener(ExitClicked);
    }

    private void StartClicked()
    {
        SceneManager.LoadScene(BattleSceneName, LoadSceneMode.Single);
    }

    private void ExitClicked()
    {
        Application.Quit();
    }
}
