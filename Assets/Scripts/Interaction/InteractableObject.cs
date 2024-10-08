using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RadiantAI;

public class InteractableObject : MonoBehaviour
{
    public enum ActionType { Mine, Chop, Eat, Sit, Sleep }
    public ActionType type;

    public Resource resource;
    public int yield;
    
    public float baseGatherTime;

    public static readonly HashSet<InteractableObject> MineableObjects = new HashSet<InteractableObject>();
    public static readonly HashSet<InteractableObject> ChoppableObjects = new HashSet<InteractableObject>();

    private void Awake()
    {
        AddToHashSet(type);
    }

    public void AddToHashSet(ActionType type)
    {
        switch (type)
        {
            case ActionType.Mine:
                MineableObjects.Add(this);
                break;
            case ActionType.Chop:
                ChoppableObjects.Add(this);
                break;
            default:
                break;
        }
    }

    public void RemoveFromHashSet(ActionType type)
    {
        switch (type)
        {
            case ActionType.Mine:
                MineableObjects.Remove(this);
                break;
            case ActionType.Chop:
                ChoppableObjects.Remove(this);
                break;
            default:
                break;
        }
    }

    private void OnDestroy()
    {
        RemoveFromHashSet(type);
    }


    public void beginInteraction(Entity interactingEntity)
    {
        interactingEntity.startGatherResource(resource, yield, baseGatherTime);
    }


}
