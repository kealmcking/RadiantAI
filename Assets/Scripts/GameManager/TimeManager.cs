using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;
    public enum Day { Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday }

    public enum TimeOfDay { Night, Dawn, Day, Dusk }

    [Header("Days / Hours / Minutes")]
    [SerializeField] private Day currentDay;

    [SerializeField] private int hoursPerDay;
    [SerializeField] private int timePerHour;

    [SerializeField] private float currentHour;
    [SerializeField] private int currentMinute;

    [SerializeField] private float timer;

    [SerializeField] private TimeOfDay timeOfDay;

    [Header("Sun/Moon")] 
    [SerializeField] private Light sunLight;
    [SerializeField] private Light moonLight;
    [SerializeField] private LightingPreset preset;
    [SerializeField] private Material daySkyBox;
    [SerializeField] private Material nightSkyBox;

    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
     
        currentDay = Day.Monday;
    }

    public float getCurrentHour()
    {
        return currentHour;
    }

    public int getCurrentMinute()
    {
        return currentMinute;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        currentMinute = (int)((timer % timePerHour) * (60f / timePerHour));

        float fractionalHour = currentHour + (timer / timePerHour);
        float timePercent = fractionalHour / hoursPerDay;
        advanceTime();

        if (preset == null)
            return;
        
        SetTimeOfDay();
        updateLighting(timePercent);
    }

    private void advanceTime()
    {
        if (timer >= timePerHour)
        {
            currentHour++;
            timer = 0;
        }

        if (currentHour >= hoursPerDay)
        {
            currentHour = 0;
            changeDay();
        }
    }
    

    private void SetTimeOfDay()
    {
        if ((currentHour > 18 && currentHour <= 24) && currentHour < 6)
        {
            timeOfDay = TimeOfDay.Night;
        } else if (currentHour >= 6 && currentHour < 9)
        {
            timeOfDay = TimeOfDay.Dawn;
        } else if (currentHour >= 9 && currentHour < 16)
        {
            timeOfDay = TimeOfDay.Day;
        } else if (currentHour >= 16 && currentHour < 18)
        {
            timeOfDay = TimeOfDay.Dusk;
        }
    }
    
    
    private void updateLighting(float timePercent)
    {
        RenderSettings.ambientLight = preset.ambientColor.Evaluate(timePercent);
        RenderSettings.fogColor = preset.fogColor.Evaluate(timePercent);

        if (sunLight != null)
        {
            sunLight.color = preset.directionalColor.Evaluate(timePercent);
            sunLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, -170f, 0));

            if (sunLight.transform.localRotation.x < 0 && sunLight.transform.localRotation.x > -180)
            {
                sunLight.enabled = false;
                RenderSettings.skybox = nightSkyBox;
            }
            else
            {
                sunLight.enabled = true;
                RenderSettings.skybox = daySkyBox;
            }
        }
    }

    private void changeDay()
    {
        switch (currentDay)
        {
            case Day.Monday:
                currentDay = Day.Tuesday;
                break;
            case Day.Tuesday:
                currentDay = Day.Wednesday;
                break;
            case Day.Wednesday:
                currentDay = Day.Thursday;
                break;
            case Day.Thursday:
                currentDay = Day.Friday;
                break;
            case Day.Friday:
                currentDay = Day.Saturday;
                break;
            case Day.Saturday:
                currentDay = Day.Sunday;
                break;
            case Day.Sunday:
                currentDay = Day.Monday;
                break;
        }
    }

    public Day getCurrentDay()
    {
        return currentDay;
    }
    
    public TimeOfDay getTimeOfDay()
    {
        return timeOfDay;
    }
}
