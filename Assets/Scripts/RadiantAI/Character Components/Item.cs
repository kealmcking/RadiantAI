using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Item", menuName = "Scriptables/Item")]
public class Item : ScriptableObject
{
    public enum ItemType { Weapon, Food, Clothing, Torch, Resource }
    
    public string itemName;
    
    public ItemType itemType;
    
    public float itemWeight;
    public float itemCost;
    
    public bool illegalItem;

    public GameObject itemModel;

    public bool isEquipped;

}
