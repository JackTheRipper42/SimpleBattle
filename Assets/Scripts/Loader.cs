using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    private const string MainMenuScene = "main";
    private const string GameOverScene = "gameOver";
    private const string TestMissionScene = "testMission";

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(MainMenuScene, LoadSceneMode.Single);
    }

    public void LoadTestMission()
    {
        SceneManager.LoadScene(TestMissionScene, LoadSceneMode.Single);
    }

    public void LoadGameOver()
    {
        SceneManager.LoadScene(GameOverScene, LoadSceneMode.Single);
    }
}