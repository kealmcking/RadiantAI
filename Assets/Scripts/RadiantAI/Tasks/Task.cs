using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RadiantAI.Tasks
{
    [CreateAssetMenu(fileName = "Task", menuName = "RadiantAI/Tasks/TaskObject")]
    public class Task : ScriptableObject
    {
        public enum TaskType { Eat, Sleep, Travel, Chat, Wait, Sit }
        
        [Header("Task Information")]
        public string taskName;
        public TaskType type;

        public Task arrivalTask;

        public Vector3 goal;
        
        public int startHour;
        public int startMinute;
    }
    
}


