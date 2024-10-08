using System;
using System.Collections.Generic;
using RadiantAI.Tasks;
using UnityEngine;

namespace RadiantAI
{
    public class CharacterSchedule : MonoBehaviour
    {
        public WeeklyTaskSchedule weeklySchedule;
        private Entity entity;

        private EventBinding<AddNewScheduleEvent> addNewSchedule;

        public Entity GetEntity()
        {
            return entity;
        }

        private void Awake()
        {
            EventBus<AddNewScheduleEvent>.Raise(new AddNewScheduleEvent()
            {
                schedule = this
            });
        }

        private void Start()
        {
            entity = GetComponent<Entity>();
        }
        
    }
}