using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;
    public enum Day { Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday }
    [SerializeField] private Day currentDay;

    [SerializeField] private int hoursPerDay;
    [SerializeField] private int timePerHour;

    [SerializeField] private int currentHour;
    [SerializeField] private int currentMinute;

    [SerializeField] private float timer;

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

    public int getCurrentHour()
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
        advanceTime();
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
}
