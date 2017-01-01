using System.Collections;
using UnityEngine;

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
        FindObjectOfType<Loader>().LoadMainMenu();
    }
}

