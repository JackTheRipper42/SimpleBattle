using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public string MainSceneName;

    private void Start()
    {
        StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSecondsRealtime(2f);
        SceneManager.LoadScene(MainSceneName, LoadSceneMode.Single);
    }
}

