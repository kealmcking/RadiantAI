using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
[CreateAssetMenu(fileName = "Resource", menuName = "Scriptables/Resource")]
public class Resource : Item
{
    public enum ResourceType { Ore, Wood, Produce, Meat}
    
    [Header("Resource Info")]
    public ResourceType type;

}
