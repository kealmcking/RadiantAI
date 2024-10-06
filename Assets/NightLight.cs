using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightLight : MonoBehaviour
{

    private Light light;
    // Start is called before the first frame update
    void Awake()
    {
        light = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if (light.enabled)
        {
            if (TimeManager.instance.getTimeOfDay() == TimeManager.TimeOfDay.Dawn || TimeManager.instance.getTimeOfDay() == TimeManager.TimeOfDay.Day)
            {
                light.enabled = false;
            }
        }
        else
        {
            if (TimeManager.instance.getTimeOfDay() == TimeManager.TimeOfDay.Dusk || TimeManager.instance.getTimeOfDay() == TimeManager.TimeOfDay.Night)
            {
                light.enabled = true;
            }
        }
    }
}
