using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RadiantAI.Tasks;

namespace RadiantAI.Scheduling
{
    [CreateAssetMenu(fileName = "Schedule", menuName = "RadiantAI/ScheduleObject")]
    public class Schedule : ScriptableObject
    {
        public List<Task> scheduledTasksMonday;
        public List<Task> scheduledTasksTuesday;
        public List<Task> scheduledTasksWednesday;
        public List<Task> scheduledTasksThursday;
        public List<Task> scheduledTasksFriday;
        public List<Task> scheduledTasksSaturday;
        public List<Task> scheduledTasksSunday;
        
        /* Eventually I would like to have some sort of menu where I can
         * lay out the schedules manually and ensure there are no overlapping
         * appointments.
         */
    }
    
}
