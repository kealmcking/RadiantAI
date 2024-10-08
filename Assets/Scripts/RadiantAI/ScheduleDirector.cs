using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RadiantAI.Tasks;

namespace RadiantAI
{
    public class ScheduleDirector : MonoBehaviour
    {
        [SerializeField] private List<CharacterSchedule> characterSchedules;

        private EventBinding<TimeChangeEvent> timeChangeEventBinding;
        private EventBinding<AddNewScheduleEvent> addNewScheduleEventBinding;

        private void OnEnable()
        {
            timeChangeEventBinding = new EventBinding<TimeChangeEvent>(OnTimeChanged);
            EventBus<TimeChangeEvent>.Register(timeChangeEventBinding);

            addNewScheduleEventBinding = new EventBinding<AddNewScheduleEvent>(OnNewScheduleAdded);
            EventBus<AddNewScheduleEvent>.Register(addNewScheduleEventBinding);
        }

        private void OnDisable()
        {
            EventBus<TimeChangeEvent>.Deregister(timeChangeEventBinding);
            EventBus<AddNewScheduleEvent>.Deregister(addNewScheduleEventBinding);
        }

        private void OnNewScheduleAdded(AddNewScheduleEvent scheduleEvent)
        {
            characterSchedules.Add(scheduleEvent.schedule);
        }

        private void OnTimeChanged(TimeChangeEvent timeEvent)
        {
            // 1. Loop through list of schedules
            foreach (CharacterSchedule schedule in characterSchedules)
            {
                // 2. If schedule has no tasks for today, do not continue
                List<Task> tasksForToday = schedule.weeklySchedule.GetTasksForDay(timeEvent.currentDay);

                if (tasksForToday != null)
                {
                    // 3. Otherwise, look through each task and see if it corresponds to the appropriate time
                    foreach (Task task in tasksForToday)
                    {
                        if (task.startHour == timeEvent.currentHour && task.startMinute == timeEvent.currentMinute)
                        {
                            // 4. If so, execute the task
                            task.ExecuteTask(schedule.GetEntity());
                        }
                    }
                    
                }
            }
        }
    }
}
