using UnityEngine;

public class MessageTrigger : MonoBehaviour
{
    public bool Finished { get; private set; }

    protected virtual void Start()
    {
        Finished = false;
    }

    protected virtual void Update()
    {
        if (Input.anyKey)
        {
            Finished = true;
        }
    }
}