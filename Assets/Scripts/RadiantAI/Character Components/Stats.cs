using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    [Header("Attributes")]
    [Range(0, 10)] public int strength;
    [Range(0, 10)] public int intelligence;
    [Range(0, 10)] public int willpower;
    [Range(0, 10)] public int agility;
    [Range(0, 10)] public int speed;
    [Range(0, 10)] public int endurance;
    [Range(0, 10)] public int personality;
    [Range(0, 10)] public int luck;

    [Header("")]
    [Range(0, 100)] public int health;
    [Range(0, 100)] public int magic;
    [Range(0, 100)] public int fatigue;
    [Range(0, 100)] public int encumbrance;
}
