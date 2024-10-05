using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "RadiantAI/StatAttribute", fileName = "StatAttribute")]
public class StatAttribute : ScriptableObject
{
    public string statName;
    public int statLevel;
}
