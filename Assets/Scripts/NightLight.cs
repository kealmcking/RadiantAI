using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightLight : MonoBehaviour
{

    private Light light;
    
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
    
    // Start is called before the first frame update
    void Awake()
    {
        light = GetComponent<Light>();
    }

    private void OnTimeChanged(TimeChangeEvent timeEvent)
    {
        if (light.enabled)
        {
            if (timeEvent.timeOfDay is TimeManager.TimeOfDay.Dawn or TimeManager.TimeOfDay.Day)
            {
                SetLightStatus(false);
            }
        } else
        {
            if (timeEvent.timeOfDay is TimeManager.TimeOfDay.Dusk or TimeManager.TimeOfDay.Night)
            {
                SetLightStatus(true);
            }
        }
        
    }

    private void SetLightStatus(bool set)
    {
        light.enabled = set;
    }
}
