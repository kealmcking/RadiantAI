using System.Collections.Generic;
using RadiantAI.Tasks;
using UnityEngine;

namespace RadiantAI
{
    [CreateAssetMenu(fileName = "NewWeeklySchedule", menuName = "RadiantAI/Schedule/WeeklyTaskSchedule")]
    public class WeeklyTaskSchedule : ScriptableObject
    {
        public List<Task> mondayTasks;
        public List<Task> tuesdayTasks;
        public List<Task> wednesdayTasks;
        public List<Task> thursdayTasks;
        public List<Task> fridayTasks;
        public List<Task> saturdayTasks;
        public List<Task> sundayTasks;

        public List<Task> GetTasksForDay(TimeManager.Day day)
        {
            switch (day)
            {
                case TimeManager.Day.Monday: return mondayTasks;
                case TimeManager.Day.Tuesday: return tuesdayTasks;
                case TimeManager.Day.Wednesday: return wednesdayTasks;
                case TimeManager.Day.Thursday: return thursdayTasks;
                case TimeManager.Day.Friday: return fridayTasks;
                case TimeManager.Day.Saturday: return saturdayTasks;
                case TimeManager.Day.Sunday: return sundayTasks;
                default: return null;
            }
        }
    }
}