using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    [SerializeField] private TMP_Text timeText;

    private EventBinding<TimeChangeEvent> timeChangeEventBinding;

    private void OnEnable()
    {
        timeChangeEventBinding = new EventBinding<TimeChangeEvent>(OnTimeChanged);
        EventBus<TimeChangeEvent>.Register(timeChangeEventBinding);
    }

    private void OnDisable()
    {
        EventBus<TimeChangeEvent>.Deregister(timeChangeEventBinding);
    }

    private void OnTimeChanged(TimeChangeEvent timeEvent)
    {
        timeText.text = $"{timeEvent.currentDay}: {timeEvent.currentHour.ToString("00")}:{timeEvent.currentMinute.ToString("00")}";
    }
}
