using UnityEngine;

public class Persistent : MonoBehaviour
{
    protected virtual void Start()
    {
        DontDestroyOnLoad(this);
    }
}