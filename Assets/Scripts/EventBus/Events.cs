using RadiantAI;
using RadiantAI.Tasks;
using UnityEngine;

public class Events : IEvent { }

public struct TestEvent : IEvent { }

public struct EntityEvent : IEvent
{
    public int health;
    public int mana;
}

public struct DoTaskEvent : IEvent
{
    public Task task;
}

public struct ChangeEquipmentEvent : IEvent
{
    public GameObject equipmentObject;
    public Item equipmentItem;
}

public struct AddNewScheduleEvent : IEvent
{
    public CharacterSchedule schedule;
}

public struct TimeChangeEvent : IEvent
{
    public TimeManager.Day currentDay;
    public TimeManager.TimeOfDay timeOfDay;
    public float currentHour;
    public int currentMinute;
}