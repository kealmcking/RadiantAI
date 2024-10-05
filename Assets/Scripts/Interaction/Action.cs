using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Action", menuName = "RadiantAI/Actions")]
public class Action : ScriptableObject
{
    [Header("Action Info")]
    public string actionName;                       // The name of the action
    
    [Header("Action Animation")]
    public string actionAnimationTriggerString;     // For if you want to use the animator
    public AnimationClip actionAnimation;               // For if you want to just play an animation clip
    
    public enum ActionType { Mine, Craft, Sit, Eat, Sleep, Lift, Chop}
    
    [Header("Other")]
    public ActionType type;                         // The type of action
}
