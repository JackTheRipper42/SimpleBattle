using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{
    public GameObject ChoisePrefab;
    public GameObject MessagePrefab;

    public CustomYieldInstruction ShowMessage(string message)
    {
        var messageText = CreateMessage(message);
        return new WaitForMessageFinished(5f, () => Destroy(messageText));
    }

    public ChoiseResult ShowChoices(params string[] choices)
    {
        var choiseGameObjects = new List<GameObject>();
        var selectedIndex = -1;
        for (var index = 0; index < choices.Length; index++)
        {
            var currentIndex = index;
            choiseGameObjects.Add(CreateChoise(index, choices[index], () => selectedIndex = currentIndex));
        }
        return new ChoiseResult(
            new WaitForChoiseFinished(
                () => selectedIndex >= 0,
                () =>
                {
                    foreach (var choiseGameObject in choiseGameObjects)
                    {
                        Destroy(choiseGameObject);
                    }
                }),
            () => selectedIndex);
    }

    private GameObject CreateChoise(int index, string message, UnityAction clickCallback)
    {
        const float width = 300f;
        const float height = 90f;
        var obj = Instantiate(ChoisePrefab);
        obj.transform.SetParent(transform);
        var button = obj.GetComponent<Button>();
        button.onClick.AddListener(clickCallback);
        var recttransform = (RectTransform) button.transform;
        recttransform.anchoredPosition = new Vector2(0, -index*height);
        recttransform.sizeDelta = new Vector2(width, height);
        var buttonText = button.GetComponentInChildren<Text>();
        buttonText.text = message;
        return obj;
    }

    private GameObject CreateMessage(string message)
    {
        const float width = 300f;
        const float height = 90f;
        var obj = Instantiate(MessagePrefab);
        obj.transform.SetParent(transform);
        var text = obj.GetComponentInChildren<Text>();
        text.text = message;
        var recttransform = (RectTransform)obj.transform;
        recttransform.anchoredPosition = new Vector2(0, 0);
        recttransform.sizeDelta = new Vector2(width, height);
        return obj;
    }

    private class WaitForMessageFinished : CustomYieldInstruction
    {
        private readonly float _waitTime;
        private Action _callback;

        public override bool keepWaiting
        {
            get
            {
                var finished = Time.realtimeSinceStartup >= _waitTime;
                if (finished && _callback != null)
                {
                    _callback();
                    _callback = null;
                }
                return !finished;
            }
        }

        public WaitForMessageFinished(float time, Action callback)
        {
            _callback = callback;
            _waitTime = Time.realtimeSinceStartup + time;
        }
    }

    private class WaitForChoiseFinished : CustomYieldInstruction
    {
        private readonly Func<bool> _finished;
        private Action _callback;

        public WaitForChoiseFinished(Func<bool> finished, Action callback)
        {
            _finished = finished;
            _callback = callback;
        }

        public override bool keepWaiting
        {
            get
            {
                var finished = _finished();
                if (finished && _callback != null)
                {
                    _callback();
                    _callback = null;
                }
                return !finished;
            }
        }
    }
}