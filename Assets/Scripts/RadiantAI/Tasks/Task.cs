using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace RadiantAI.Tasks
{
    [CreateAssetMenu(fileName = "Task", menuName = "RadiantAI/Tasks/TaskObject")]
    public class Task : ScriptableObject
    {
        public enum TaskType { Eat, Sleep, Travel, Chat, Wait, Sit, Retrieve, Deliver }
        
        [Header("Task Information")]
        public string taskName;
        public TaskType type;
        public NpcAction npcAction;

        // IMPLEMENT LATER - IF NECESSARY
        //public Task arrivalTask;
        
        public int startHour;
        public int startMinute;

        private bool isMoving;


        public void ExecuteTask(Entity entity)
        {
            Debug.Log($"{entity.name} is performing {taskName} at hour {startHour} and minute {startMinute}");
            // Do task logic here
            entity.GetNavMeshAgent().destination = entity.nearestInteractableObjects(returnHashSet(npcAction.type)).gameObject.transform.position;

            entity.StartCoroutine(entity.CheckArrival(this));
        }
        
        private HashSet<InteractableObject> returnHashSet(NpcAction.ActionType action)
        {
            switch (action)
            {
                case NpcAction.ActionType.Mine:
                    return InteractableObject.MineableObjects;
                case NpcAction.ActionType.Chop:
                    return InteractableObject.ChoppableObjects;
                default:
                    return null;
            }
        }
    }
    
}


