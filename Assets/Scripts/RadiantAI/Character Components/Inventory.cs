using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    [Header("Inventory")] 
    [SerializeField] internal float maxWeight;

    internal List<ItemEntry> inventory = new List<ItemEntry>();

    [System.Serializable]
    public class ItemEntry
    {
        public Item item;
        public int amount;
    }


    public void addItemToInventory(Item _newItem, int _amount)
    {
        if (_newItem == null)
        {
            return;
        }
        
        foreach (var entry in inventory)
        {
            if (entry.item == _newItem)
            {
                entry.amount += _amount;
                return;
            }
        }
        
        inventory.Add(new ItemEntry{ item = _newItem, amount = _amount });
    }
    
    
    public bool isOverEncumbered()
    {
        float currentWeight = currentInventoryWeight();
        return currentWeight > maxWeight;
    }


    public float currentInventoryWeight()
    {
        float currentWeight = 0;
        
        foreach (var entry in inventory)
        {
            currentWeight += entry.item.itemWeight * entry.amount;
        }

        return currentWeight;
    }
    
}
